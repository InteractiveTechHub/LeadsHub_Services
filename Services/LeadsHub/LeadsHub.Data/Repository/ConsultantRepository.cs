
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using AdaptiveKitCore.Responses.Interfaces;
using Dapper;
using LeadsHub.Core.Extentions;
using LeadsHub.Core.Identity.Models;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Reflection.Metadata;

namespace LeadsHub.Data.Repository
{
    public sealed class ConsultantRepository : IConsultantRepository
    {
        private const string selectConsultant = "SELECT c.\"Id\",  "
                                                    + "c.\"IdentityId\", "
                                                    + "c.\"FullName\", "
                                                    + "c.\"NickName\", "
                                                    + "u.\"Id\", "
                                                    + "u.\"AccessFailedCount\", "
                                                    + "u.\"Email\", "
                                                    + "u.\"EmailConfirmed\", "
                                                    + "u.\"Enabled\", "
                                                    + "u.\"LockoutEnd\", "
                                                    + "u.\"PhoneNumber\", "
                                                    + "u.\"UserName\", "
                                                    + "u.\"EmailConfirmed\", "
                                                    + "r.\"Id\", "
                                                    + "r.\"Name\", "
                                                    + "cp.\"Id\", "
                                                    + "cp.\"BrandName\" "
                                                    + "FROM \"Consultant\" c "
                                                    + "RIGHT JOIN \"AspNetUsers\" u ON c.\"IdentityId\" = u.\"Id\" "
                                                    + "LEFT JOIN \"AspNetUserRoles\" ur ON ur.\"UserId\" = u.\"Id\" "
                                                    + "LEFT JOIN \"AspNetRoles\" r ON ur.\"RoleId\" = r.\"Id\" "
                                                    + "LEFT JOIN \"ConsultantCompany\" ct ON c.\"Id\" = ct.\"ConsultantId\" "
                                                    + "LEFT JOIN \"Company\" cp ON cp.\"Id\" = ct.\"CompanyId\" ";

        private const string selectConsultantCount = "SELECT COUNT(1) "
                                                        + "FROM \"Consultant\" c "
                                                        + "RIGHT JOIN \"AspNetUsers\" u ON c.\"IdentityId\" = u.\"Id\" "
                                                        + "LEFT JOIN \"AspNetUserRoles\" ur ON ur.\"UserId\" = u.\"Id\" "
                                                        + "LEFT JOIN \"AspNetRoles\" r ON ur.\"RoleId\" = r.\"Id\""
                                                        + "LEFT JOIN \"ConsultantCompany\" ct ON c.\"Id\" = ct.\"ConsultantId\" "
                                                        + "LEFT JOIN \"Company\" cp ON cp.\"Id\" = ct.\"CompanyId\" ";

        private readonly IDbConnection _dbConnection;

        public ConsultantRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ModelResponse> CreatesConsultantAsync(Consultant consultant)
        {
            ModelResponse response = new();

            try
            {
                string insertConsultnatCommand = "INSERT INTO \"Consultant\" (\"IdentityId\", \"FullName\", \"NickName\", \"PhotoUrl\", \"Enabled\") VALUES (@IdentityId, @FullName, @NickName, @PhotoUrl, @Enabled) RETURNING \"Id\"";
                string insertCommand = "INSERT INTO \"ConsultantCompany\" (\"IdentityId\", \"ConsultantId\", \"CompanyId\") VALUES (@IdentityId, @ConsultantId, @CompanyId)";

                _dbConnection.Open();
                using var transaction = _dbConnection.BeginTransaction();

                long consultantId = await _dbConnection.ExecuteScalarAsync<long>(insertConsultnatCommand, consultant, transaction);
                if (consultantId == 0) 
                {
                    transaction.Rollback();

                    response.AddErrorMessage("ConsultantCreateError");
                }

                int count = 0;
                foreach (Company company in consultant.Companies)
                {
                    ConsultantCompany consultantCompany = new() 
                    { 
                        ConsultantId = consultantId,
                        IdentityId = consultant.IdentityId,
                        CompanyId = company.Id
                    };

                    count = await _dbConnection.ExecuteAsync(insertCommand, consultantCompany, transaction);
                    if (count == 0) 
                    {
                        transaction.Rollback();

                        response.AddErrorMessage($"ConsultantCompanyInsertError");
                    }
                }

                transaction.Commit();
            }
            catch(Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<ConsultantResponse> FetchConsultantsByRequestAsync(FilterRequest filterRequest)
        {
            ConsultantResponse response = new();

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

                string querySql = string.Join(' ', selectConsultant, WhereClause, sortExpression, offset);
                string querySqlCount = string.Join(' ', selectConsultantCount, WhereClause);

                IEnumerable<Consultant> result = await _dbConnection.QueryAsync<Consultant, ApplicationUser, IdentityRole, Company, Consultant>(querySql, 
                    (consultant, applicationUser, role, company) =>
                    {
                        if (applicationUser is not null)
                        {
                            consultant.UserIdentity = new(applicationUser, role);
                        }

                        if (company is not null)
                        {
                            consultant.Companies.Add(company);
                        }

                        return consultant;
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

        public async Task<SimpleResponse<Consultant>> FetchConsultantByUserIdAsync(string userId)
        {
            SimpleResponse<Consultant> response = new();

            try
            {
                string querySql = $"SELECT * FROM \"Consultant\" WHERE \"IdentityId\" = '{userId}'";

                Consultant? consultant = await _dbConnection.QueryFirstAsync<Consultant>(querySql);

                response.Model = consultant;
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage($"{ex.Message}");
            }

            return response;
        }

        public async Task<ModelResponse> UpdateConsultantAsync(Consultant consultant)
        {
            ModelResponse response = new();

            // TODO: Update de companies
            string updateConsultantCommand = "UPDATE \"Consultant\" SET \"FullName\"=@FullName, \"NickName\"=@NickName, \"PhotoUrl\"=@PhotoUrl, \"Enabled\"=@Enabled, \"IdentityId\"=@IdentityId WHERE \"Id\"=@Id";
            string deleteConsultantCompaniesCommand = "DELETE FROM \"ConsultantCompany\" WHERE \"ConsultantId\" = @ConsultantId";
            string insertConsultantCompaniesCommand = "INSERT INTO \"ConsultantCompany\" (\"IdentityId\", \"ConsultantId\", \"CompanyId\") VALUES (@IdentityId, @ConsultantId, @CompanyId)";

            try
            {
                _dbConnection.Open();
                using var transaction = _dbConnection.BeginTransaction();

                int result = await _dbConnection.ExecuteAsync(updateConsultantCommand, consultant, transaction);
                if (result == 0)
                {
                    response.AddErrorMessage("Erro ao atulizar consultor");
                    transaction.Rollback();

                    return response;
                }

                int deleteResult = await _dbConnection.ExecuteAsync(deleteConsultantCompaniesCommand, 
                    new { ConsultantId = consultant.Id }, transaction);               

                int companyResult = 0;
                foreach (var company in consultant.Companies)
                {
                    ConsultantCompany consultantCompany = new()
                    {
                        ConsultantId = consultant.Id,
                        IdentityId = consultant.IdentityId,
                        CompanyId = company.Id
                    };

                    companyResult += await _dbConnection.ExecuteAsync(insertConsultantCompaniesCommand, consultantCompany, transaction);
                }

                if (companyResult == 0)
                {
                    transaction.Rollback();
                    return response;
                }

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
