
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
    public sealed class SalesPipelineRepository : ISalesPipelineRepository
    {
        private const string query = "SELECT " +
                "sp.\"Id\", " +
                "sp.\"Name\", " +
                "sp.\"CompanyId\", " +
                "sp.\"ConsultantId\", " +
                "stg.\"Id\", " +
                "stg.\"Title\", " +
                "stg.\"StageOrder\"," +
                "ltg.\"Id\", " +
                "ltg.\"LeadId\", " +
                "ltg.\"PipelineStageId\", " +
                "ltg.\"Position\"," +
                "ltg.\"MovedAt\", " +
                "c.\"Id\", " +
                "c.\"Name\" AS LeadName, " +
                "c.\"PhoneNumber\", " +
                "c.\"Email\", " +
                "ld.\"Id\" As LeadId," +
                "ld.\"Identifier\", " +
                "ld.\"CompanyId\", " +
                "ld.\"CreatedAt\", " +
                "cs.\"Id\" AS ConsultantId," +
                "cs.\"IdentityId\" AS UserIdentityId, " +
                "cs.\"FullName\" AS ConsultantName, " +
                "lm.\"LastMessage\", " +
                "lm.\"LastMessageDate\"," +
                "lm.\"Status\" " +
                "FROM \"SalesPipeline\" sp " +
                "INNER JOIN \"PipelineStage\" stg ON sp.\"Id\" = stg.\"SalesPipelineId\" " +
                "LEFT JOIN \"LeadStage\" ltg ON stg.\"Id\" = ltg.\"PipelineStageId\" " +
                "LEFT JOIN \"Lead\" ld ON  ltg.\"LeadId\" = ld.\"Id\" " +
                "LEFT JOIN \"Contact\" c ON ld.\"ContactId\" = c.\"Id\" " +
                "LEFT JOIN \"Consultant\" cs ON ld.\"ConsultantId\" = cs.\"Id\" " +
                "LEFT JOIN \"LastMessage\" lm ON lm.\"LeadId\" = ld.\"Id\" " +
                "WHERE sp.\"Id\" = @PipelineId";

        public async Task<SimpleResponse<SalesPipeline>> CreatePipelineAsync(SalesPipeline salesPipeline)
        {
            SimpleResponse<SalesPipeline> response = new();

            const string createCommand = "INSERT INTO \"SalesPipeline\" (\"CompanyId\", \"ConsultantId\", \"Name\") VALUES (@CompanyId, @ConsultantId, @Name) RETURNING \"Id\" ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                long pipelineId = await connection.ExecuteScalarAsync<long>(createCommand, salesPipeline, transaction);
                if (pipelineId == 0) 
                {
                    response.AddErrorMessage("Not created");

                    return response;
                }                

                salesPipeline.Id = pipelineId;             
                salesPipeline.Stages.ForEach(s => s.SalesPipelineId = pipelineId);

                salesPipeline = await CreatePipelineStageAsync(salesPipeline, connection, transaction);
                response.Model = salesPipeline;

                response.AddSuccessMessage("Susscessfully created");

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<SimpleResponse<LeadStage>> CreateLeadStageAsync(LeadStage leadStage)
        {
            SimpleResponse<LeadStage> response = new();

            const string shiftPositionsCommand = "UPDATE \"LeadStage\" SET \"Position\" = \"Position\" + 1 WHERE \"PipelineStageId\" = @PipelineStageId;";

            const string createCommand = "INSERT INTO \"LeadStage\" (\"LeadId\", \"PipelineStageId\", \"Position\") VALUES (@LeadId, @PipelineStageId, 0) RETURNING \"Id\"; ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                await connection.ExecuteAsync(shiftPositionsCommand, leadStage, transaction);

                long leadStageId = await connection.ExecuteScalarAsync<long>(createCommand, leadStage, transaction);
                if (leadStageId == 0)
                {
                    response.AddErrorMessage("Not created");

                    return response;
                }

                leadStage.Id = leadStageId;
                leadStage.Position = 0;
                response.Model = leadStage;

                response.AddSuccessMessage("Susscessfully created");

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<LeadStageResponse> FetchLeadStageByRequest(FilterRequest filterRequest)
        {
            LeadStageResponse response = new();

            string selectLeadStage = "SELECT * FROM \"LeadStage\" ";
            string selectLeadStageCount = "SELECT COUNT(1) FROM \"LeadStage\" ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                string WhereClause = filterRequest.BuildWhereClause();

                string sortExpression = string.Empty;
                foreach (var sort in filterRequest.SortExpressions)
                {
                    sortExpression = $"ORDER BY {sort.PropertyName} {sort.SortDirection}";
                }

                string offset = "";
                if (filterRequest.PageSize > 0)
                {
                    offset += $"OFFSET {filterRequest.Skip} ROWS FETCH NEXT {filterRequest.PageSize} ROW ONLY;";
                }

                string querySql = string.Join(' ', selectLeadStage, WhereClause, sortExpression, offset);
                string querySqlCount = string.Join(' ', selectLeadStageCount, WhereClause);

                IEnumerable<LeadStage> result = await connection.QueryAsync<LeadStage>(querySql);

                if (result.Any() && filterRequest.PageSize > 0)
                {
                    int totalCount = await connection.QueryFirstOrDefaultAsync<int>(querySqlCount);
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

        public async Task<SimpleResponse<SalesPipeline>> FetchPipelineByIdAsync(long pipelineId)
        {
            SimpleResponse<SalesPipeline> response = new();             

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                var stageDict = new Dictionary<long, PipelineStage>();
                SalesPipeline? pipeline = null;

                var result = await connection.QueryAsync<SalesPipeline, PipelineStage, LeadStage, LeadCard, SalesPipeline>(
                    query,
                    (sp, stage, leadStage, leadCard) =>
                    {
                        if (pipeline == null)
                        {
                            pipeline = sp;
                        }

                        if (!stageDict.TryGetValue(stage.Id, out var currentStage))
                        {
                            currentStage = stage;
                            currentStage.Leads = new List<LeadStage>();
                            pipeline.Stages.Add(currentStage);
                            stageDict[stage.Id] = currentStage;
                        }

                        if (leadStage != null && leadStage.Id != 0)
                        {
                            leadStage.LeadCard = leadCard;
                            currentStage.Leads.Add(leadStage);
                        }

                        return pipeline;
                    },
                    param: new { PipelineId = pipelineId },
                    splitOn: "Id"
                );

                response.Model = pipeline ?? new();
            }
            catch (Exception ex)
            {
                // Handle exception
                response.AddExceptionMessage(ex.Message);
                Console.WriteLine($"Error fetching pipeline: {ex.Message}");
            }

            return response;
        }

        public async Task<SalesPipelineResponse> FetchPipelinesByRequestAsync(FilterRequest filterRequest)
        {
            SalesPipelineResponse response = new();

            string selectSalesPipeline = "SELECT * FROM \"SalesPipeline\" ";
            string selectSalesPipelineCount = "SELECT COUNT(1) FROM \"SalesPipeline\" ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                string WhereClause = filterRequest.BuildWhereClause();

                string sortExpression = string.Empty;
                foreach (var sort in filterRequest.SortExpressions)
                {
                    sortExpression = $"ORDER BY {sort.PropertyName} {sort.SortDirection}";
                }

                string offset = "";
                if (filterRequest.PageSize > 0)
                {
                    offset += $"OFFSET {filterRequest.Skip} ROWS FETCH NEXT {filterRequest.PageSize} ROW ONLY;";
                }

                string querySql = string.Join(' ', selectSalesPipeline, WhereClause, sortExpression, offset);
                string querySqlCount = string.Join(' ', selectSalesPipelineCount, WhereClause);

                IEnumerable<SalesPipeline> result = await connection.QueryAsync<SalesPipeline>(querySql);

                if (result.Any() && filterRequest.PageSize > 0)
                {
                    int totalCount = await connection.QueryFirstOrDefaultAsync<int>(querySqlCount);
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

        public async Task<PipelineStageResponse> FetchPipelineStageByPipeIdAsync(long salesPipelineId)
        {
            PipelineStageResponse response = new();

            string selectStage = "SELECT * " +
                "FROM \"PipelineStage\" ps " +
                "LEFT JOIN \"LeadStage\" ls ON ls.\"PipelineStageId\" = ps.\"Id\" " +
                "WHERE \"SalesPipelineId\" = @SalesPipelineId ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                IEnumerable<PipelineStage> result = await connection.QueryAsync<PipelineStage, LeadStage, PipelineStage>(selectStage, 
                    (pipeStage, leadStage) => 
                    {
                        if (leadStage is not null)
                        {
                            pipeStage.Leads.Add(leadStage);
                        }                       

                        return pipeStage;
                    }, param: new { salesPipelineId }, splitOn: "Id");

                response.ResponseData = [.. result];
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<ModelResponse> UpdatePipelinesAsync(List<SalesPipeline> salesPipelineList)
        {
            ModelResponse response = new();

            string updateCommand = "UPDATE \"SalesPipeline\" SET \"CompanyId\"=@CompanyId, \"ConsultantId\"=@ConsultantId, \"Name\"=@Name WHERE \"Id\" = @Id;";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                int result = await connection.ExecuteAsync(updateCommand, salesPipelineList, transaction);
                if (result == 0)
                {
                    response.AddErrorMessage("Error while updating");
                }

               await transaction.CommitAsync();
            }
            catch(Exception ex) 
            {
                // log exception
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<ModelResponse> UpdatePipelineStageAsync(PipelineStage stage)
        {
            ModelResponse response = new();

            string updateCommand = "UPDATE \"PipelineStage\" SET \"Title\"=@Title, \"StageOrder\"=@StageOrder WHERE \"Id\" = @Id;";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                int result = await connection.ExecuteAsync(updateCommand, stage, transaction);
                if (result == 0)
                {
                    response.AddErrorMessage("Error while updating");
                }
            }
            catch (Exception ex)
            {
                // log exception
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<ModelResponse> UpdateLeadStageAsync(IEnumerable<LeadStage> leadStageList)
        {
            ModelResponse response = new();

            string updateCommand = "UPDATE \"LeadStage\" SET \"PipelineStageId\"=@PipelineStageId, \"Position\"=@Position, \"MovedAt\"=@MovedAt WHERE \"Id\"=@Id;";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                int result = await connection.ExecuteAsync(updateCommand, leadStageList, transaction);                
                if (result == 0)
                {
                    response.AddErrorMessage("Error while updating");

                    return response;
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // log exception
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        private async Task<SalesPipeline> CreatePipelineStageAsync(SalesPipeline salesPipeline, NpgsqlConnection connection, NpgsqlTransaction transaction)
        {
            const string createCommand = "INSERT INTO \"PipelineStage\" (\"SalesPipelineId\", \"Title\", \"StageOrder\") VALUES (@SalesPipelineId, @Title, @StageOrder);";

            var result = await connection.ExecuteAsync(createCommand, salesPipeline.Stages, transaction);

            return salesPipeline;
        }
    }
}
