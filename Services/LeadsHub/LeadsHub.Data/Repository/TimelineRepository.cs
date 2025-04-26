using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using Dapper;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Utility;
using Npgsql;

namespace LeadsHub.Data.Repository
{
    public sealed class TimelineRepository : ITimelineRepository
    {
        private const string selectTimeline = "SELECT " +
            "t.\"Id\", " +
            "t.\"LeadId\", " +
            "t.\"ConsultantId\", " +
            "t.\"Sender\", " +
            "t.\"MessageDate\", " +
            "t.\"ReadAt\", " +
            "t.\"Status\", " +
            "t.\"Type\", " +
            "t.\"MessageTextId\", " +
            "t.\"MessageFileId\", " +
            "mt.\"Id\", " +
            "mt.\"Body\", " +
            "mf.\"Id\", " +
            "mf.\"Url\", " +
            "mf.\"Caption\", " +
            "mr.\"Id\", " +
            "mr.\"Emoji\" " +
            "FROM \"Timeline\" t " +
            "LEFT JOIN \"MessageText\" mt ON mt.\"Id\" = t.\"MessageTextId\" " +
            "LEFT JOIN \"MessageFile\" mf ON mf.\"Id\" = t.\"MessageFileId\" " +
            "LEFT JOIN \"MessageReaction\" mr ON mr.\"Id\" = t.\"MessageReactionId\" " +
            "WHERE t.\"LeadId\" = @LeadId ORDER BY \"MessageDate\"";

        private const string selectTimelineCount = "SELECT COUNT(1) " +
            "FROM \"Timeline\" t " +
            "LEFT JOIN \"MessageText\" mt ON mt.\"Id\" = t.\"MessageTextId\" " +
            "LEFT JOIN \"MessageFile\" mf ON mf.\"Id\" = t.\"MessageFileId\" " +
            "LEFT JOIN \"MessageReaction\" mr ON mr.\"Id\" = t.\"MessageReactionId\" WHERE t.\"LeadId\" = @LeadId";
        
        private const string insertUpdateLastMessage = "INSERT INTO \"LastMessage\" " +
            "(\"LeadId\", " +
            "\"TimelineId\", " +
            "\"LastMessage\", " +
            "\"LastMessageDate\", " +
            "\"Status\") " +
            "VALUES " +
            "(@LeadId, " +
            "@TimelineId, " +
            "@LastMessage, " +
            "@LastMessageDate, " +
            "@Status) " +
            "ON CONFLICT (\"LeadId\") " +
            "DO UPDATE SET \"TimelineId\" = EXCLUDED.\"TimelineId\", " +
            "\"LastMessage\" = EXCLUDED.\"LastMessage\", " +
            "\"LastMessageDate\" = EXCLUDED.\"LastMessageDate\", " +
            "\"Status\" = EXCLUDED.\"Status\"";

        public async Task<SimpleResponse<Timeline>> FetchTimelineOnlyByRequestAsync(FilterRequest filterRequest)
        {
            SimpleResponse<Timeline> response = new();

            string query = "SELECT * FROM \"Timeline\" ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                string WhereClause = filterRequest.BuildWhereClause();

                query += WhereClause;

                Timeline? result = await connection.QueryFirstOrDefaultAsync<Timeline>(query);

                response.Model = result ?? new();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<TimelineResponse> FetchTimelineByRequestAsync(long leadId, FilterRequest filterRequest)
        {
            TimelineResponse response = new();

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                string offset = "";
                if (filterRequest.PageSize > 0)
                {
                    offset += $"OFFSET {filterRequest.Skip} ROWS FETCH NEXT {filterRequest.PageSize} ROW ONLY;";
                }

                string querySql = string.Join(' ', selectTimeline, offset);

                // Apply multi-query
                IEnumerable<Timeline> result = await connection.QueryAsync<Timeline, MessageText, MessageFile, MessageReaction, Timeline>(querySql, 
                    (timeline, messageText, messageFile, messageReaction) =>
                    {
                        timeline.Message = messageText;
                        timeline.MessageFile = messageFile;
                        timeline.MessageReaction = messageReaction;

                        return timeline;
                    },
                    param: new { LeadId = leadId },
                    splitOn: "Id");

                if (result.Any() && filterRequest.PageSize > 0)
                {
                    int totalCount = await connection.QueryFirstOrDefaultAsync<int>(selectTimelineCount, new { LeadId = leadId });
                    response.TotalAvailableItems = totalCount;
                }

                response.ResponseData.AddRange(result);
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<SimpleResponse<Timeline>> RegisterMessageTextAsync(Timeline timeline)
        {
            string insertMessageText = "INSERT INTO \"MessageText\" (\"Body\") VALUES (@Body) RETURNING \"Id\"";

            SimpleResponse<Timeline> response = new();

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                timeline.MessageTextId = await connection.ExecuteScalarAsync<long>(insertMessageText, timeline.Message, transaction);
                if (timeline.MessageTextId > 0)
                {
                    timeline = await RegisterTimelineAsync(timeline, transaction);
                }

                LastMessageSet lastMessage = new()
                {
                    LeadId = timeline.LeadId,
                    TimelineId = timeline.Id,
                    LastMessage = timeline.Message!.Body,
                    LastMessageDate = timeline.MessageDate,
                    Sender = timeline.Sender,
                    Status = timeline.Status
                };

                await connection.ExecuteScalarAsync(insertUpdateLastMessage, lastMessage, transaction);

                response.Model = timeline;

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // TODO: implement log
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<SimpleResponse<Timeline>> RegisterMessageFileAsync(Timeline timeline)
        {
            string insertFile = "INSERT INTO \"MessageFile\" (\"MimeType\", \"Caption\", \"Url\") VALUES (@MimeType, @Caption, @Url) RETURNING \"Id\"";
            
            SimpleResponse<Timeline> response = new();

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                timeline.MessageFileId = await connection.ExecuteScalarAsync<long>(insertFile, timeline.MessageFile, transaction);
                if (timeline.MessageFileId > 0)
                {
                    timeline.MessageFile!.Id = timeline.MessageFileId.Value;

                    timeline = await RegisterTimelineAsync(timeline, transaction);
                }

                response.Model = timeline;

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // TODO: implement log
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task UpdateTimelineAsync(Timeline timeline)
        {
            string updateCommand = "UPDATE \"Timeline\" SET \"ConsultantId\"=@ConsultantId, \"MessageId\"=@MessageId, \"Sender\"=@Sender, \"Status\"=@Status, \"ReadAt\"=@ReadAt, \"UpdatedAt\"=@UpdatedAt WHERE \"Id\"=@Id;";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                var result = await connection.ExecuteAsync(updateCommand, timeline, transaction);
                if (result == 0)
                {
                    // Not updated
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // TODO: Log here
                string errorMessage = ex.Message;
            }
        }

        private async Task<Timeline> RegisterTimelineAsync(Timeline timeline, NpgsqlTransaction transaction)
        {
            string insertTimeline = "INSERT INTO \"Timeline\" (\"LeadId\", \"ConsultantId\", \"Sender\", \"Type\", \"Status\", \"MessageId\", \"MessageDate\", \"ReadAt\", \"MessageTextId\", \"MessageFileId\") " +
                "VALUES (@LeadId, @ConsultantId, @Sender, @Type, @Status, @MessageId, @MessageDate, @ReadAt, @MessageTextId, @MessageFileId) RETURNING \"Id\"";

            try
            {
                timeline.Id = await transaction.Connection!.ExecuteScalarAsync<long>(insertTimeline, timeline, transaction);
            }
            catch (Exception ex)
            {
                var teste = ex.Message;
                transaction.Rollback();
            }          

            return timeline;
        }
    }
}
