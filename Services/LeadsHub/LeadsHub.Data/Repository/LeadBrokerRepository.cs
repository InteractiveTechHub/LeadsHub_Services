
using AdaptiveKitCore.Responses;
using Dapper;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Utility;
using Npgsql;

namespace LeadsHub.Data.Repository
{
    public sealed class LeadBrokerRepository : ILeadBrokerRepository
    {
        const string querySelectConsultantInLine = "SELECT "
                                        + "cs.\"Id\", " 
                                        + "cs.\"IdentityId\", "
                                        + "cs.\"FullName\", "
                                        + "cs.\"Enabled\", "
                                        + "cs.\"TimeLastLeadAssigned\", "
                                        + "c.\"Id\" AS CompanyId "
                                        + "FROM \"Consultant\" cs "
                                        + "INNER JOIN \"ConsultantCompany\" ct ON ct.\"ConsultantId\" = cs.\"Id\" "
                                        + "INNER JOIN \"Company\" c ON c.\"Id\" = ct.\"CompanyId\" "
                                        + "WHERE cs.\"Enabled\" = true "
                                        + "AND c.\"Id\" = @CompanyId "
                                        + "ORDER BY cs.\"TimeLastLeadAssigned\" ASC NULLS FIRST "
                                        + "LIMIT 1;";

        string selectLeadByContact = "SELECT ld.\"Id\", " +
                       "ld.\"Identifier\", " +
                       "ld.\"CompanyId\", " +
                       "ld.\"ConsultantId\", " +
                       "ld.\"ContactId\", " +
                       "ld.\"CampaignId\", " +
                       "ld.\"AdCode\", " +
                       "ld.\"Channel\", " +
                       "ld.\"Status\", " +
                       "ct.\"Id\", " +
                       "ct.\"IdentityId\" "
               + "FROM \"Lead\" ld "
               + "INNER JOIN \"Contact\" c ON ld.\"ContactId\" = c.\"Id\" "
               + "LEFT JOIN \"Consultant\" ct ON ct.\"Id\" = ld.\"ConsultantId\" "
               + "WHERE (c.\"PhoneNumber\" = @PhoneNumber AND c.\"PhoneNumber\" <> '') "
               + "OR (c.\"Email\" = @Email AND c.\"Email\" <> '') "
               + "OR (c.\"CPF\" = @CPF AND c.\"CPF\" <> '')";

        public async Task<SimpleResponse<Consultant>> FetchNextConsultant(long companyId)
        {
            SimpleResponse<Consultant> response = new();

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                Consultant? consultant = await connection.QueryFirstOrDefaultAsync<Consultant>(querySelectConsultantInLine, new { CompanyId = companyId });

                response.Model = consultant;
            }
            catch (Exception ex)
            {
                // should register a log
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<SimpleResponse<Lead>> RegisterLeadAsync(Lead lead)
        {
            SimpleResponse<Lead> response = new();

            string insertLead = "INSERT INTO \"Lead\" (\"ContactId\", \"CompanyId\", \"ConsultantId\", \"Status\", \"Channel\", \"IntegrationId\") ";
            string insertLead2 = $"{insertLead} VALUES (@ContactId, @companyid, @ConsultantId, @Status, @Channel, @IntegrationId) RETURNING \"Id\", \"Identifier\";";
                  
            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                var result = await connection.QueryFirstAsync<dynamic>(insertLead2, lead, transaction);

                lead.Id = result?.Id;
                lead.Identifier = result?.Identifier ?? string.Empty;
                response.Model = lead;

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // should store a log
                response.AddExceptionMessage(ex.Message);              
            }

            return response;
        }

        public async Task UpdateConsultantsAsync(Consultant consultant)
        {
            string updateQuery = "UPDATE \"Consultant\" SET \"TimeLastLeadAssigned\" = @TimeLastLeadAssigned WHERE \"Id\" = @Id";

            try
            {

                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                var result = await connection.ExecuteScalarAsync(updateQuery, consultant, transaction);

               await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                // should register a log
            }
        }

        /// <summary>
        /// Fetch lead
        /// </summary>
        /// <param name="contact">The data to find the lead</param>
        /// <returns>Return Existing Lead</returns>
        public async Task<SimpleResponse<Lead?>> FetchLeadByContactAsync(Contact contact)
        {
            SimpleResponse<Lead?> response = new();         

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                var result = await connection.QueryAsync<Lead, Consultant, Lead>(selectLeadByContact, (lead, consultant) =>
                {
                    lead.Consultant = consultant;
                    return lead;
                },
                param: new { contact.PhoneNumber, contact.Email, contact.CPF },
                splitOn: "Id");

                response.Model = result.FirstOrDefault();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<SimpleResponse<Timeline>> FetchTimelineAsync(Timeline timeline)
        {
            SimpleResponse<Timeline> response = new();

            string query = "SELECT * FROM \"Timeline\" WHERE \"LeadId\" = @LeadId AND \"ConsultantId\" = @ConsultantId ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                Timeline? result = await connection.QueryFirstOrDefaultAsync<Timeline>(query, timeline);

                response.Model = result ?? new();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }       
    }
}
