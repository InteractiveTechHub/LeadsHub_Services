
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class IntegrationController : BaseController
    {
        private readonly IIntegrationBac _integrationBac;

        public IntegrationController(IIntegrationBac integrationBac)
        {
            _integrationBac = integrationBac;
        }

        [HttpPost]
        public async Task<IActionResult> FetchAllIntegrationsAsync(FilterRequest filterRequest)
        {
            IntegrationResponse response = await _integrationBac.FetchIntegrationsByRequestAsync(filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
