
using AdaptiveKitCore.Requests;
using WhatsApp.Core.Models;
using WhatsApp.Core.Response;

namespace WhatsApp.Core.Interfaces.IRepository
{
    public interface IWhatsAppConfigRepository
    {
        Task<ConfigResponse> FetchConfigByCompanyIdAsync(long companyId);

        Task<BaseResponse<Integration>> FetchWhatsappConfigByRequestAsync(FilterRequest filterRequest);
    }
}
