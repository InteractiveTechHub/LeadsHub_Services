
using Dapper;
using System.Data;
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Interfaces.IRepository;
using AdaptiveKitCore.Responses;

namespace LeadsHub.Data.Repository
{
    public class WhatsAppRepository : IWhatsAppRepository
    {

        private string selectIntegration = "SELECT " +
            "i.\"Id\", " +
            "i.\"CompanyId\", " +
            "w.\"Id\", " +
            "w.\"PhoneNumberId\", " +
            "w.\"BusinessAccountId\", " +
            "w.\"AccessToken\", " +
            "wt.\"Id\", " +
            "wt.\"Name\", " +
            "wt.\"TemplateBodyMirror\", " +
            "wt.\"Type\", " +
            "wt.\"Variables\", " +
            "wt.\"Enabled\" " +
            "FROM \"Integration\" i " +
            "INNER JOIN \"WhatsAppConfig\" w ON i.\"WhatsAppConfigId\" = w.\"Id\"" +
            "LEFT JOIN \"WhatsAppTemplate\" wt ON w.\"Id\" = wt.\"WhatsAppConfigId\" ";

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

                var result = await _dbConnection.QueryAsync<Integration, WhatsAppConfig, WhatsAppTemplate, Integration>(query, 
                    (integration, whatsapp, WhatsAppTemplate) =>
                {
                    integration.WhatsAppConfig = whatsapp;

                    if (WhatsAppTemplate is not null)
                    {
                        integration.WhatsAppConfig.WhatsAppTemplates.Add(WhatsAppTemplate);
                    }            

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
        
        public async Task<SimpleResponse<WhatsAppTemplate>> FetchWhatsAppTemplateByIdAsync(long id)
        {
            SimpleResponse<WhatsAppTemplate> response = new();

            try
            {
                string query = "SELECT * FROM \"WhatsAppTemplate\" WHERE \"Id\" = @Id ";

                WhatsAppTemplate? result = await _dbConnection.QueryFirstOrDefaultAsync<WhatsAppTemplate>(query, new { Id = id});
                if (result is null)
                {
                    response.AddErrorMessage("Template not found");
                    return response;
                }

                response.Model = result;
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }
    }
}
