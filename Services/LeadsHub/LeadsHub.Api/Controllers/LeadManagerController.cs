
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
            UserContext userContext = await _userContextService.GetUserContextAsync();

            FilterRequest filterRequest = new();

            if (request.IsLeadCreatedAtDesc)
            {
                filterRequest.AddSortExpressionDescending(nameof(Lead.CreatedAt), "ld");
            }
            else if (request.IsLeadCreatedAtAsc)
            {
                filterRequest.AddSortExpressionAscending(nameof(Lead.CreatedAt), "ld");
            }
            else if (request.IsInteractionDesc)
            {
                filterRequest.AddSortExpressionDescending(nameof(LastMessageSet.LastMessageDate), "lm");
            }
            else if (request.IsInteractionAsc)
            {
                filterRequest.AddSortExpressionAscending(nameof(LastMessageSet.LastMessageDate), "lm");
            }

            userContext.FilterRequest.ShorthandSortExpressions = filterRequest.ShorthandSortExpressions;

            LeadCardResponse response = await _leadManagerBac.FetchCardsByRequestAsync(userContext.FilterRequest);
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
