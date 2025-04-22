
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface ILeadManagerBac
    {
        Task<SimpleResponse<Lead?>> CloseLeadAsync(LeadCard lead);

        Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest);

        Task<BaseResponse<TemplatesPerType>> FetchTemplatesAsync(long leadId);
    }
}
