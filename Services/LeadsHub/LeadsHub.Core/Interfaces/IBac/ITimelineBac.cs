
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ITimelineBac
    {
        Task<TimelineResponse> FetchTimelineByRequestAsync(long timelineId, FilterRequest filterRequest);

        Task<SimpleResponse<Timeline>> RegisterTimelineAsync(Timeline message);
    }
}
