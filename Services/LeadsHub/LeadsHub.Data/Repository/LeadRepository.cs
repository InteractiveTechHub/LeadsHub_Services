
using AdaptiveKitCore.Responses;
using Dapper;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Utility;
using Npgsql;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LeadsHub.Data.Repository
{
    public sealed class LeadRepository : ILeadRepository
    {
        string query = "SELECT ld.\"Id\", "
            + "ld.\"Identifier\" "
            + "ld.\"CompanyId\", "
            + "ld.\"ConsultantId\", "
            + "ld.\"ContactId\", "
            + "ld.\"CampaignId\", "
            + "ld.\"AdCode\", "
            + "ld.\"Channel\", "
            + "ld.\"Status\", "
            + "ld.\"IntegrationId\", "
            + "c.\"Id\", "
            + "c.\"Name\", "
            + "c.\"PhoneNumber\", "
            + "c.\"Email\" "
            + "FROM public.\"Lead\" ld "
            + "INNER JOIN \"Contact\" c ON ld.\"ContactId\" = c.\"Id\" WHERE ld.\"Id\" = @LeadId";

        public async Task<SimpleResponse<Lead?>> FetchLeadByIdAsync(long leadId)
        {
            SimpleResponse<Lead?> response = new();

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                var result = await connection.QueryAsync<Lead, Contact, Lead>(query,
                    (lead, contact) =>
                    {
                        lead.Contact = contact;

                        return lead;
                    }, param: new { LeadId = leadId },
                    splitOn: "Id");

                response.Model = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }
    }
}
