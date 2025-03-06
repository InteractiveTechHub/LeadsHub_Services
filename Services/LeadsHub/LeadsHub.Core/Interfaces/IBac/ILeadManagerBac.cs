
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ILeadManagerBac
    {
        Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest);
    }
}
