
using AdaptiveKitCore.Requests;
using Dapper;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using System.Data;

namespace LeadsHub.Data.Repository
{
    public sealed class IntegrationRepository : IIntegrationRepository
    {
        private readonly IDbConnection _dbConnection;

        public IntegrationRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IntegrationResponse> FetchIntegrationsByRequestAsync(FilterRequest filterRequest)
        {
            IntegrationResponse response = new();

            try
            {
                string selectIntegration = "SELECT * FROM \"Integration\" i LEFT JOIN \"WhatsAppConfig\" w ON i.\"WhatsAppConfigId\" = w.\"Id\"";
                string selectIntegrationCount = "SELECT COUNT(1) FROM \"Integration\" i LEFT JOIN \"WhatsAppConfig\" w ON i.\"WhatsAppConfigId\" = w.\"Id\"";

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

                string querySql = string.Join(' ', selectIntegration, WhereClause, sortExpression, offset);
                string querySqlCount = string.Join(' ', selectIntegrationCount, WhereClause);

                IEnumerable<Integration> result = await _dbConnection.QueryAsync<Integration, WhatsAppConfig, Integration>(querySql,
                    (integration, whatsAppConfig) =>
                    {
                        if (whatsAppConfig is not null)
                        {
                            integration.WhatsAppConfig = whatsAppConfig;
                        }

                        return integration;
                    }, splitOn: "Id");

                if (result.Any() && filterRequest.PageSize > 0)
                {
                    response.TotalAvailableItems = await _dbConnection.QueryFirstOrDefaultAsync<int>(querySqlCount);
                }

                response.ResponseData.AddRange(result);
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage($"{ex.Message}");
            }

            return response;
        }
    }
}
