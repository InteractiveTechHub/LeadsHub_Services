
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Bac
{
    public sealed class LeadBac : ILeadBac
    {
        private readonly ILeadRepository _leadRepository;
        public LeadBac(ILeadRepository leadRepository)
        {
            _leadRepository = leadRepository;
        }


        public async Task<SimpleResponse<Lead?>> FetchLeadByIdAsync(long leadId)
        {
            return await _leadRepository.FetchLeadByIdAsync(leadId);
        }
    }
}
