
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ILeadManagerBac
    {
        Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest);

        Task<BaseResponse<TemplatesPerType>> FetchTemplatesAsync(long leadId);
    }
}
