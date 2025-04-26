using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using AdaptiveKitCore.Responses.Interfaces;
using LeadsHub.Api.Services;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Enum;
using LeadsHub.Core.Identity;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using Microsoft.AspNetCore.Mvc;

namespace LeadsHub.Api.Controllers
{
    public class TimelineController : BaseController
    {
        private readonly ILeadBac _leadBac;
        private readonly ITimelineBac _timelineBac;
        private readonly IUserContextService _userContextService;

        public TimelineController(ILeadBac leadBac, ITimelineBac timelineBac, IUserContextService userContextService)
        {
            _leadBac = leadBac;
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

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessageAsync([FromBody] Timeline timeline)
        {
            UserContext user = await _userContextService.GetUserContextAsync();
            
            var leadResponse = await FetchLeadByIdAsync(timeline.LeadId);
            if (leadResponse.HasAnyErrorMessage)
            {
                return BadRequest(leadResponse);
            }

            timeline.MessageDate = timeline.MessageDate.ToUniversalTime();
            timeline.ConsultantId = user.ConsultantId;
            timeline.LeadId = leadResponse.Model.Id;

            TimelineFormData formData = new()
            {
                Timeline = timeline,
                Lead = leadResponse.Model,
            };

            SimpleResponse<Timeline> respose = await _timelineBac.RegisterTimelineAsync(formData);
            if (respose.HasAnyErrorMessage)
            {
                return BadRequest(respose);
            }

            return Ok(respose);
        }

        [HttpPost("upload/{leadId:long}")]
        public async Task<IActionResult> SendFilesAsync([FromForm] List<IFormFile> files, [FromForm] List<string> captions, long leadid)
        {
            UserContext user = await _userContextService.GetUserContextAsync();

            var leadResponse = await FetchLeadByIdAsync(leadid);
            if (leadResponse.HasAnyErrorMessage)
            {
                return BadRequest(leadResponse);
            }

            List<Timeline> timelines = [];

            for (int index = 0; index < files.Count; index++)
            {
                Timeline timeline = new()
                {
                    MessageDate = DateTimeOffset.UtcNow.ToUniversalTime(),
                    ConsultantId = user.ConsultantId,
                    Type = DetectMessageType(files[index]),
                    MessageFile = new()
                    {
                        Caption = captions.ElementAtOrDefault(index) ?? string.Empty,
                        MimeType = files[index].ContentType
                    }
                };

                TimelineFormData formData = new()
                {
                    Timeline = timeline,
                    Lead = leadResponse.Model,
                    FormFile = files[index]
                };

                SimpleResponse<Timeline> respose = await _timelineBac.RegisterTimelineAsync(formData);
                if (respose.HasAnyErrorMessage)
                {
                    return BadRequest(respose);
                }
            }

            return Ok();
        }

        private async Task<SimpleResponse<Lead>> FetchLeadByIdAsync(long leadId)
        {
            SimpleResponse<Lead?> response = await _leadBac.FetchLeadByIdAsync(leadId);
            if (response.HasAnyErrorMessage)
            {
                return response!;
            }

            if (response.Model is null || response.Model.Id == 0)
            {
                response.AddErrorMessage("Lead not found");
                return response!;
            }

            return response!;
        }

        private MessageType DetectMessageType(IFormFile file)
        {
            var contentType = file.ContentType.ToLower();

            return contentType switch
            {
                string type when type.StartsWith("image/") => MessageType.Image,
                string type when type.StartsWith("video/") => MessageType.Video,
                string type when type.StartsWith("audio/") => MessageType.Audio,
                string type when type == "application/pdf" => MessageType.Document,
                string type when type.StartsWith("application/") => MessageType.Document,

                _ => MessageType.Document // fallback
            };
        }
    }
}
