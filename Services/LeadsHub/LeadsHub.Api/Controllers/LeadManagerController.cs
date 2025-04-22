
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using LeadsHub.Api.Services;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Identity;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{    public class LeadManagerController : BaseController
    {
        private readonly ILeadManagerBac _leadManagerBac;
        private readonly IUserContextService _userContextService;
        public LeadManagerController(ILeadManagerBac leadManagerBac, IUserContextService userContextService)
        {
            _leadManagerBac = leadManagerBac;
            _userContextService = userContextService;
        }

        [HttpPost("leadcards")]
        public async Task<IActionResult> FetchLeadCardsByRequestAsync(ManagerFilterRequest request)
        {
            FilterRequest filterRequest = new();

            if (request.IsLeadCreatedAtDesc)
            {
                filterRequest.AddSortExpressionDescending(nameof(LeadCard.CreatedAt), "ld");
            }
            else if (request.IsLeadCreatedAtAsc)
            {
                filterRequest.AddSortExpressionAscending(nameof(LeadCard.CreatedAt), "ld");
            }
            else if (request.IsInteractionDesc)
            {
                filterRequest.AddSortExpressionDescending(nameof(LeadCard.LastMessageDate), "lm");
            }
            else if (request.IsInteractionAsc)
            {
                filterRequest.AddSortExpressionAscending(nameof(LeadCard.LastMessageDate), "lm");
            }

            if (!string.IsNullOrWhiteSpace(request.GlobalFilter))
            {
                filterRequest.AddFilter(nameof(Contact.Name), FilterOperatorEnum.Contains, FilterConnectorEnum.AND, request.GlobalFilter, "con", ignoreAccent: true);
                filterRequest.AddFilter(nameof(Contact.PhoneNumber), FilterOperatorEnum.Contains, FilterConnectorEnum.OR, request.GlobalFilter, "con", ignoreAccent: true);
                filterRequest.AddFilter(nameof(Contact.Email), FilterOperatorEnum.Contains, FilterConnectorEnum.OR, request.GlobalFilter, "con", ignoreAccent: true);
                filterRequest.AddFilter(nameof(Consultant.FullName), FilterOperatorEnum.Contains, FilterConnectorEnum.OR, request.GlobalFilter, "c", ignoreAccent: true);
            }

            UserContext userContext = await _userContextService.GetUserContextAsync();
            filterRequest.AddFilterDescriptors(userContext.FilterRequest.FilterDescriptors);

            LeadCardResponse response = await _leadManagerBac.FetchCardsByRequestAsync(filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("templates")]
        public async Task<IActionResult> FetchTemplatesAsync(long leadId)
        {
            BaseResponse<TemplatesPerType> response = await _leadManagerBac.FetchTemplatesAsync(leadId);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("close-lead")]
        public async Task<IActionResult> CloseLeadAsync(LeadCard leadCard)
        {
            var response = await _leadManagerBac.CloseLeadAsync(leadCard);
            if (response.HasAnyErrorMessage) 
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
