using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class CompanyController : BaseController
    {
        private readonly ICompanyBac _companyBac;
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyBac companyBac, ICompanyService companyService)
        {
            _companyBac = companyBac;
            _companyService = companyService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompanyAsync([FromBody] Company company)
        {
            ModelResponse response = await _companyBac.CreateCompanyAsync(company);
            if (response.HasErrorMessage || response.HasExceptionMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("fetchall")]
        public async Task<IActionResult> FetchCompanyByRequestAsync(FilterRequest filterRequest) 
        {
            CompanyResponse response = await _companyBac.FetchCompanyByRequestAsync(filterRequest);

            return Ok(response);
        }

        [HttpGet("comapnyData")]
        public async Task<IActionResult> FindCompanyDataAsync(string cnpj)
        {
            SimpleResponse<Company> response = await _companyService.GetDataByCNPJAsync(cnpj);
            if (response.HasErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("companyAddress")]
        public async Task<IActionResult> FindCompanyAddressAsync(string cep)
        {
            SimpleResponse<Address> response = await _companyService.GetAddressByCEP(cep);
            if (response.HasErrorMessage) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCompanyAsync(Company company)
        {
            ModelResponse response = await _companyBac.UpdateCompanyAsync(company);
            if (response.HasErrorMessage || response.HasExceptionMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
