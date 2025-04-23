
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ITimelineBac
    {
        Task<TimelineResponse> FetchTimelineByRequestAsync(long timelineId, FilterRequest filterRequest);

        Task<SimpleResponse<Timeline>> RegisterTimelineAsync(TimelineFormData formData);
    }
}
