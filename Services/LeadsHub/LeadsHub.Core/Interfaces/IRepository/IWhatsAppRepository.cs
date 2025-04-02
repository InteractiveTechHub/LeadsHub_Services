
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IRepository
{
    public interface IWhatsAppRepository
    {
        Task<BaseResponse<Integration>> FetchWhatsappConfigByRequestAsync(FilterRequest filterRequest);

        Task<SimpleResponse<WhatsAppTemplate>> FetchWhatsAppTemplateByIdAsync(long id);
    }
}
