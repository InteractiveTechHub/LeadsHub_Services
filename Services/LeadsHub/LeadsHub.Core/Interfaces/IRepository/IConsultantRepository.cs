

using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface IConsultantRepository
    {
        Task<ModelResponse> CreatesConsultantAsync(Consultant consultant);

        Task<ConsultantResponse> FetchConsultantsByRequestAsync(FilterRequest filterRequest);

        Task<ModelResponse> UpdateConsultantAsync(Consultant consultant);
    }
}
