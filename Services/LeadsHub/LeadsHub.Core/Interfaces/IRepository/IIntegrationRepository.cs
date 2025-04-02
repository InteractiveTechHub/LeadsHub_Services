
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface IIntegrationRepository
    {
        Task<IntegrationResponse> FetchIntegrationsByRequestAsync(FilterRequest filterRequest);
    }
}
