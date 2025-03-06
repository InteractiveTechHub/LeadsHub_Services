

using AdaptiveKitCore.Requests;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ILeadManagerRepository
    {
        Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest);
    }
}
