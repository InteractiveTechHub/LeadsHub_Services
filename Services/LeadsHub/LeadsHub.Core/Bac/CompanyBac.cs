
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Validations;

namespace LeadsHub.Core.Bac
{
    public sealed class CompanyBac : ICompanyBac
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly CompanyValidator _validator;
        public CompanyBac(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
            _validator = new();
        }

        public async Task<ModelResponse> CreateCompanyAsync(Company company)
        {
            ModelResponse response = new();

            // TODO: Applies SOLID here
            var result = await _validator.ValidateAsync(company);
            if (!result.IsValid) 
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.ErrorMessage);
                }

                return response;
            }         

            return await _companyRepository.CreateCompanyAsync(company);
        }

        /// <summary>
        /// Fetch all companies using pagination
        /// </summary>
        /// <param name="filterRequest">The filter for the search</param>
        /// <returns>Companies filtered</returns>
        public async Task<CompanyResponse> FetchCompanyByRequestAsync(FilterRequest filterRequest)
        {
            CompanyResponse response = await _companyRepository.FetchCompanyByRequestAsync(filterRequest);

            return response;
        }

        public async Task<ModelResponse> UpdateCompanyAsync(Company company)
        {
            ModelResponse response = new();

            // TODO: Applies SOLID here
            var result = await _validator.ValidateAsync(company);
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    response.AddErrorMessage(error.ErrorMessage);
                }

                return response;
            }

            return await _companyRepository.UpdateCompanyAsync(company);
        }
    }
}
