
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Bac
{
    public sealed class LeadManagerBac : ILeadManagerBac
    {
        private readonly ILeadManagerRepository _managerRepository;

        public LeadManagerBac(ILeadManagerRepository leadManagerRepository)
        {
            _managerRepository = leadManagerRepository;
        }

        public async Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest)
        {
            return await _managerRepository.FetchCardsByRequestAsync(filterRequest);
        }
    }
}
