using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

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
            SimpleResponse<Timeline> respose = await _timelineBac.RegisterTimelineAsync(timeline);
            if (respose.HasAnyErrorMessage)
            {
                return BadRequest(respose);
            }

            // Send message to whatsapp

            return Ok(respose);
        }
    }
}
