
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Identity;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Bac
{
    public sealed class ConsultantBac : IConsultantBac
    {
        private readonly IConsultantRepository _consultantRepository;

        public ConsultantBac(IConsultantRepository consultantRepository)
        {
            _consultantRepository = consultantRepository;
        }

        public async Task<ModelResponse> CreatesConsultantAsync(Consultant consultant)
        {
            return await _consultantRepository.CreatesConsultantAsync(consultant);
        }

        public async Task<SimpleResponse<UserContext>> FetchConsultantByUserIdAsync(string userId)
        {
            var response = await _consultantRepository.FetchConsultantByUserIdAsync(userId);

            return response;
        }

        public async Task<ConsultantResponse> FetchConsultantsByRequestAsync(FilterRequest filterRequest)
        {
            return await _consultantRepository.FetchConsultantsByRequestAsync(filterRequest);
        }

        public async Task<ModelResponse> UpdateConsultantAsync(Consultant consultant)
        {
            consultant.UpdatedAt = DateTimeOffset.UtcNow;

            if (consultant.Id == 0)
            {
                FilterRequest filterRequest = new();
                filterRequest.AddFilter(nameof(Consultant.IdentityId), FilterOperatorEnum.Equals, consultant.IdentityId, "c");
                var response = await _consultantRepository.FetchConsultantsByRequestAsync(filterRequest);

                if (!response.ResponseData.Any())
                {
                    var modelResponse = await _consultantRepository.CreatesConsultantAsync(consultant);

                    return modelResponse;
                }
            }

            return await _consultantRepository.UpdateConsultantAsync(consultant);
        }
    }
}
