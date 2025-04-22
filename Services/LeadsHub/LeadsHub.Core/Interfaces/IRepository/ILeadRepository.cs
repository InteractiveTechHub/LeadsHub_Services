
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface ILeadRepository
    {
        Task<SimpleResponse<Lead?>> FetchLeadByIdAsync(long leadId);

        Task<SimpleResponse<Lead?>> UpdateLeadAsync(Lead lead);
    }
}
