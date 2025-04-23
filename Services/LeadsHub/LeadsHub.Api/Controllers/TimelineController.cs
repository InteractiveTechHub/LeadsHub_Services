using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Api.Services;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Identity;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeadsHub.Api.Controllers
{
    public class TimelineController : BaseController
    {
        private readonly IConsultantBac _consultantBac;
        private readonly ITimelineBac _timelineBac;
        private readonly IUserContextService _userContextService;

        public TimelineController(IConsultantBac consultantBac, ITimelineBac timelineBac, IUserContextService userContextService)
        {
            _consultantBac = consultantBac;
            _timelineBac = timelineBac;
            _userContextService = userContextService;
        }

        [HttpPost("{timelineId:long}")]
        public async Task<IActionResult> FetchTimelineByLeadRequestAsync(long timelineId, [FromBody] FilterRequest filterRequest)
        {
            TimelineResponse response = await _timelineBac.FetchTimelineByRequestAsync(timelineId, filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterMessagesAsync([FromBody] TimelineFormData formData)
        {
            formData.Timeline.MessageDate = formData.Timeline.MessageDate.ToUniversalTime();

            UserContext user = await _userContextService.GetUserContextAsync();

            formData.Timeline.ConsultantId = user.ConsultantId;

            SimpleResponse<Timeline> respose = await _timelineBac.RegisterTimelineAsync(formData);
            if (respose.HasAnyErrorMessage)
            {
                return BadRequest(respose);
            }

            return Ok(respose);
        }
    }
}
