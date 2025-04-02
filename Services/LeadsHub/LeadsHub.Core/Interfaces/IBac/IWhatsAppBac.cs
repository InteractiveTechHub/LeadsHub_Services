
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Interfaces.IBac
{
    public interface IWhatsAppBac
    {
        Task<BaseResponse<Integration>> FetchAllWhatsAppByRequestAsync(FilterRequest filterRequest);

        Task ReceiveMessageFromWhatsappAsync(WhatsAppPayLoad whatsappMessage);
    }
}
