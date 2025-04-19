
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using Dapper;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace LeadsHub.Data.Repository
{
    public sealed class CompanyRepository : ICompanyRepository
    {
        private const string selectCompanies = "SELECT * FROM \"Company\" c LEFT JOIN \"Address\" a ON c.\"Id\" = a.\"CompanyId\" ";
        private const string selectCompaniesCount = "SELECT COUNT(1) FROM \"Company\" c LEFT JOIN \"Address\" a ON c.\"Id\\\" = a.\"CompanyId\" \" ";

        private readonly IDbConnection _dbConnection;

        public CompanyRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ModelResponse> CreateCompanyAsync(Company company)
        {
            ModelResponse response = new();

            _dbConnection.Open();
            using var transaction = _dbConnection.BeginTransaction();

            try
            {
                string insert = "INSERT INTO \"Company\" (\"BrandName\", \"LegalName\", \"Email\", \"Enabled\", \"IdentificationNumber\", \"PhoneNumber\") VALUES (@BrandName, @LegalName, @Email, @Enabled, @IdentificationNumber, @PhoneNumber) RETURNING \"Id\"";

                int companyId = await _dbConnection.ExecuteScalarAsync<int>(insert, company, transaction);

                if (companyId == 0) 
                {
                    response.AddErrorMessage("CompanyNotSaved", "001");

                    return response;
                }

                // Persists the address
                company.Address.CompanyId = companyId;

                string Addressinsert = "INSERT INTO \"Address\" (\"CompanyId\", \"Zipcode\", \"State\", \"City\", \"Street\", \"Neighborhood\", \"Number\") VALUES (@CompanyId, @Zipcode, @State, @City, @Street, @Neighborhood, @Number)";

                int result = await _dbConnection.ExecuteAsync(Addressinsert, company.Address, transaction);
                if (result == 0) 
                {
                    response.AddErrorMessage("AddressNotSaved", "002");
                    transaction.Rollback();
                }

                transaction.Commit();

                response.AddSuccessMessage("CompanyCreated");
            }
            catch 
            {
                response.AddExceptionMessage("CompanyExceptionWhenTrySave", "001");
                transaction.Rollback();
            }

            return response;
        }

        /// <summary>
        /// Fetch all companies using pagination
        /// </summary>
        /// <param name="filterRequest">The filter for the search</param>
        /// <returns>Companies filtered</returns>
        public async Task<CompanyResponse> FetchCompanyByRequestAsync(FilterRequest filterRequest)
        {
            CompanyResponse response = new();

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

                string querySql = string.Join(' ', selectCompanies, WhereClause, sortExpression, offset);
                string querySqlCount = string.Join(' ', selectCompaniesCount, WhereClause);

                IEnumerable<Company> result = await _dbConnection.QueryAsync<Company, Address, Company>(querySql, (company, address) =>
                {
                    if (company != null) 
                    { 
                        company.Address = address;
                    }

                    return company ?? new();
                }, splitOn: "CompanyId");

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

        public async Task<ModelResponse> UpdateCompanyAsync(Company company)
        {
            ModelResponse response = new();
            string updateCommand = "UPDATE \"Company\" SET \"BrandName\" = @BrandName, \"LegalName\" = @LegalName, \"Email\" = @Email, \"Enabled\" = @Enabled, \"IdentificationNumber\" = @IdentificationNumber, \"PhoneNumber\" = @PhoneNumber WHERE \"Id\" = @Id";
            
            _dbConnection.Open();
            
            using var transaction = _dbConnection.BeginTransaction();

            try
            {                            
                int result = await _dbConnection.ExecuteAsync(updateCommand,company, transaction);

                if (result == 0)
                {
                    response.AddErrorMessage("Empresa não foi atualizada.");
                }

                // TODO: Update the Address

                response.AddSuccessMessage("Empresa foi atualizada com sucesso.");

                transaction.Commit();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage("Ocorreu um erro ao tentar atualizar empresa");
                transaction.Rollback();
            }

            return response;
        }
    }
}
