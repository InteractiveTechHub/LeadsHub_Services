using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface IConsultantBac
    {
        Task<ModelResponse> CreatesConsultantAsync(Consultant consultant);

        Task<ConsultantResponse> FetchConsultantsByRequestAsync(FilterRequest filterRequest);

        Task<ModelResponse> UpdateConsultantAsync(Consultant consultant);
    }
}
