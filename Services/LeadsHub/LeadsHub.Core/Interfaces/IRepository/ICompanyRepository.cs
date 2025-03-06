
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ICompanyRepository
    {
        Task<ModelResponse> CreateCompanyAsync(Company company);

        /// <summary>
        /// Fetch all companies using pagination
        /// </summary>
        /// <param name="filterRequest">The filter for the search</param>
        /// <returns>Companies filtered</returns>
        Task<CompanyResponse> FetchCompanyByRequestAsync(FilterRequest filterRequest);

        Task<ModelResponse> UpdateCompanyAsync(Company company);
    }
}
