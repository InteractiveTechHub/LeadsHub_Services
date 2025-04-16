
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ITimelineRepository
    {
        Task<TimelineResponse> FetchTimelineByRequestAsync(long leadId, FilterRequest filterRequest);

        Task<SimpleResponse<Timeline>> FetchTimelineOnlyByRequestAsync(FilterRequest filterRequest);

        Task<SimpleResponse<Timeline>> RegisterMessageTextAsync(Timeline timeline);

        Task<SimpleResponse<Timeline>> RegisterMessageFileAsync(Timeline timeline);

        Task UpdateTimelineAsync(Timeline timeline);
    }
}
