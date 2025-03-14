
using Dapper;
using System.Data;
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Interfaces.IRepository;

namespace WhatsApp.Data.Repository
{
    public class WhatsAppRepository : IWhatsAppRepository
    {

        private string selectIntegration = "SELECT " +
            "i.\"Id\", " +
            "i.\"CompanyId\", " +
            "w.\"Id\", " +
            "w.\"PhoneNumberId\", " +
            "w.\"BusinessAccountId\", " +
            "w.\"AccessToken\" " +
            "FROM \"Integration\" i " +
            "INNER JOIN \"WhatsAppConfig\" w ON i.\"WhatsAppConfigId\" = w.\"Id\"";

        private readonly IDbConnection _dbConnection;

        public WhatsAppRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<BaseResponse<Integration>> FetchWhatsappConfigByRequestAsync(FilterRequest filterRequest)
        {
            BaseResponse<Integration> response = new();

            try
            {
                string whereClause = filterRequest.BuildWhereClause();

                string query = string.Join(' ', selectIntegration, whereClause);

                var result = await _dbConnection.QueryAsync<Integration, WhatsAppConfig, Integration>(query, (integration, whatsapp) =>
                {
                    integration.WhatsAppConfig = whatsapp;

                    return integration;
                }, 
                splitOn: "Id");

                response.ResponseData = [.. result];
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }       

        /*public async Task<ConfigResponse> FetchConfigByCompanyIdAsync(long companyId)
        {
            ConfigResponse response = new();

            try
            {
                var result = await _dbConnection.QueryAsync<WhatsAppConfig>($"SELECT * FROM WhatsappConfig WHERE CompanyId = '{companyId}'");

                response.ResponseData = result.ToList();
            }
            catch (Exception ex)
            {
                // log here
                response.AddExceptionMessage($"WhatsAppConfigRepository.FetchWhatsAppConfigByComapnyIdAsync :: {ex.Message}");
            }

            return response;
        }*/
    }
}
