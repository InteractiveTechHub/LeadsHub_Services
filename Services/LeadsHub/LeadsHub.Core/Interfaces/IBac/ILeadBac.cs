

using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ILeadBac
    {
        Task<SimpleResponse<Lead?>> FetchLeadByIdAsync(long leadId);
    }
}
