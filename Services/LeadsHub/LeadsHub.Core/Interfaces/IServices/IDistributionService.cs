
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Models;

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface IDistributionService
    {
        Task<Consultant> DistributeLeadsAsync(long companyId);
    }
}
