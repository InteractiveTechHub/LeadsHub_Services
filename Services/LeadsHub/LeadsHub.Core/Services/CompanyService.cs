
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using System.Text.Json;

namespace LeadsHub.Core.Services
{
    public sealed class CompanyService : ICompanyService
    {
        private readonly HttpClient _httpClient;

        public CompanyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<SimpleResponse<Company>> GetDataByCNPJAsync(string cnpj)
        {
            string url = $"https://brasilapi.com.br/api/cnpj/v1/{cnpj}";

            SimpleResponse<Company> companyResponse = new();

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var cnpjData = JsonSerializer.Deserialize<CNPJDataDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Company company = BuildCompany(cnpjData);

                companyResponse.Model = company;

                return companyResponse;
            }

            companyResponse.AddErrorMessage("CNPJNotFound");

            return companyResponse;
        }

        public async Task<SimpleResponse<Address>> GetAddressByCEP(string cep)
        {
            string url = $"https://brasilapi.com.br/api/cep/v2/{cep}";

            SimpleResponse<Address> addressResponse = new();

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var cepInfo = JsonSerializer.Deserialize<CNPJDataDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                Address address = BuildAddress(cepInfo);

                addressResponse.Model = address;

                return addressResponse;
            }

            addressResponse.AddErrorMessage("CEPNotFound");

            return addressResponse;
        }

        private Company BuildCompany(CNPJDataDto cnpjData) 
        {
            Company company = new()
            {
                LegalName = cnpjData.RazaoSocial,
                BrandName = cnpjData.NomeFantasia,
                IdentificationNumber = cnpjData.Cnpj,
                Email = cnpjData.Email ?? string.Empty,  
            };

            return company;
        }

        private Address BuildAddress(CNPJDataDto cnpjData)
        {
            Address address = new()
            {
                City = cnpjData.City,
                Neighborhood = cnpjData.Neighborhood,
                State = cnpjData.State,
                Street = cnpjData.Street,                
                ZipCode = cnpjData.CEP
            };

            return address;
        }
    }
}
