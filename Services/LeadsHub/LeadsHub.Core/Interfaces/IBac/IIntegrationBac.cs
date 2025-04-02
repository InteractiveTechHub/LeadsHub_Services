
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface IIntegrationBac
    {
        Task<IntegrationResponse> FetchIntegrationsByRequestAsync(FilterRequest filterRequest);
    }
}
