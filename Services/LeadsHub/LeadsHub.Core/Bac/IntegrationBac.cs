
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Bac
{
    public sealed class IntegrationBac : IIntegrationBac
    {
        private readonly IIntegrationRepository _integrationRepository;
        public IntegrationBac(IIntegrationRepository integrationRepository)
        {
            _integrationRepository = integrationRepository;
        }

        public async Task<IntegrationResponse> FetchIntegrationsByRequestAsync(FilterRequest filterRequest)
        {
            var response = await _integrationRepository.FetchIntegrationsByRequestAsync(filterRequest);

            return response;
        }
    }
}
