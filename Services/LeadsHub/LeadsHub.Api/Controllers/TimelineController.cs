using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Identity.Models;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LeadsHub.Api.Controllers
{
    public class TimelineController : BaseController
    {
        private readonly ITimelineBac _timelineBac;

        public TimelineController(ITimelineBac timelineBac)
        {
            _timelineBac = timelineBac;
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
        public async Task<IActionResult> RegisterMessagesAsync([FromBody] Timeline timeline)
        {
            timeline.MessageDate = timeline.MessageDate.ToUniversalTime();
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("NotPermitionSendMessage");
            }

            timeline.ConsultantId = 1; //TODO: FetchConsultantByUserId(userId);

            SimpleResponse <Timeline> respose = await _timelineBac.RegisterTimelineAsync(timeline);
            if (respose.HasAnyErrorMessage)
            {
                return BadRequest(respose);
            }

            return Ok(respose);
        }
    }
}
