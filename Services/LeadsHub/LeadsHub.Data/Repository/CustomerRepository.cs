
using AdaptiveKitCore.Responses;
using Dapper;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Utility;
using Npgsql;

namespace LeadsHub.Data.Repository
{
    public sealed class CustomerRepository : ICustomerRepository
    {
        public async Task<SimpleResponse<long>> FetchContactIdAsync(Contact contact)
        {
            SimpleResponse<long> response = new();

            string query = "SELECT c.\"Id\" " +
                "FROM \"Contact\" c " +
                "WHERE c.\"PhoneNumber\" = @PhoneNumber " +
                    "OR (c.\"Email\" = @Email AND c.\"Email\" <> '') " +
                    "OR (c.\"CPF\" = @CPF AND c.\"CPF\" <> '') ";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                long customerId = await connection.ExecuteScalarAsync<long>(query, new { contact.Email, contact.CPF, contact.PhoneNumber });

                response.Model = customerId;
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<SimpleResponse<long>> RegisterContactAsync(Contact contact)
        {
            SimpleResponse<long> response = new();

            string insertCustomerCommand = "INSERT INTO \"Contact\" (\"Name\",\"PhoneNumber\", \"Email\", \"CPF\", \"BirthDate\") VALUES (@Name, @PhoneNumber, @Email, @CPF, @BirthDate) RETURNING \"Id\";";

            try
            {
                using var connection = new NpgsqlConnection(SD.ConnectString);
                await connection.OpenAsync();

                using var transaction = await connection.BeginTransactionAsync();

                long customerId = await connection.ExecuteScalarAsync<long>(insertCustomerCommand, contact, transaction);
                if (customerId == 0)
                {
                    response.AddErrorMessage("Error while trying to insert Customer");

                    transaction.Rollback();
                }

                response.Model = customerId;

                transaction.Commit();
            } 
            catch (Exception ex)
            {
               response.AddExceptionMessage(ex.Message);
            }

            return response;
        }
    }
}
