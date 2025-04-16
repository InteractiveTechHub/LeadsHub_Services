
using AdaptiveKitCore.Requests;
using Dapper;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using System.Data;

namespace LeadsHub.Data.Repository
{
    public sealed class LeadManagerRepository : ILeadManagerRepository
    {
        private const string selectLeadCards = "SELECT ld.\"Id\" As LeadId, "
                                                    + "ld.\"Identifier\", "
                                                    + "ld.\"CompanyId\", "
                                                    + "ld.\"Channel\", "
                                                    + "ld.\"Phase\", "
                                                    + "ld.\"Status\", "
                                                    + "ld.\"CreatedAt\" AS CreatedAt, "
                                                    + "c.\"Id\" As ConsultantId, "
                                                    + "c.\"IdentityId\" AS UserIdentityId, "
                                                    + "c.\"FullName\" AS ConsultantName, "
                                                    + "con.\"Name\" AS LeadName, "
                                                    + "con.\"PhoneNumber\", "
                                                    + "con.\"Email\", "
                                                    + "lm.\"LastMessage\", "
                                                    + "lm.\"LastMessageDate\", "
                                                    + "lm.\"Status\", "
                                                    + "COUNT(t.\"Id\") FILTER (WHERE t.\"Status\" = 1) As TotalNewMessages "
                                                + "FROM \"Lead\" ld "
                                                + "INNER JOIN \"Contact\" con ON con.\"Id\" = ld.\"ContactId\" "
                                                + "LEFT JOIN \"Consultant\" c ON c.\"Id\" = ld.\"ConsultantId\" "
                                                + "LEFT JOIN \"Timeline\" t ON t.\"LeadId\" = ld.\"Id\" "
                                                + "LEFT JOIN \"LastMessage\" lm ON lm.\"LeadId\" = ld.\"Id\"";

        private string groupBy = "GROUP BY ld.\"Id\", "
            + "ld.\"Identifier\", "
            + "ld.\"CompanyId\", "
            + "ld.\"Channel\", "
            + "ld.\"Phase\", "
            + "ld.\"Status\", "
            + "ld.\"CreatedAt\", "
            + "c.\"Id\", "
            + "c.\"IdentityId\", "
            + "c.\"FullName\", "
            + "con.\"Name\", "
            + "con.\"PhoneNumber\", "
            + "con.\"Email\", "
            + "lm.\"LastMessage\", "
            + "lm.\"LastMessageDate\", "
            + "lm.\"LastMessageDate\", "
            + "lm.\"Status\"";

        private const string selectLeadCount = "SELECT COUNT(*) "
                                                + "FROM \"Lead\" ld "
                                                + "INNER JOIN \"Contact\" cm ON cm.\"Id\" = ld.\"ContactId\" "
                                                + "LEFT JOIN \"Consultant\" c ON c.\"Id\" = ld.\"ConsultantId\" ";


        private readonly IDbConnection _dbConnection;

        public LeadManagerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest)
        {
            LeadCardResponse response = new();

            try
            {
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

                string querySql = string.Join(' ', selectLeadCards, WhereClause, groupBy, sortExpression, offset);
                string querySqlCount = string.Join(' ', selectLeadCount, WhereClause);

                IEnumerable<LeadCard> result = await _dbConnection.QueryAsync<LeadCard>(querySql);

                if (result.Any() && filterRequest.PageSize > 0)
                {
                    int totalCount = await _dbConnection.QueryFirstOrDefaultAsync<int>(querySqlCount);
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
    }
}
