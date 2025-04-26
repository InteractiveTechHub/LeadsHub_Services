
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads.Whatsapp.Response;
using LeadsHub.Core.Request;

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface IWhatsappService
    {
        Task<string> GetMediaFromWhatsapp(string mediaId, WhatsAppConfig config);

        Task<BaseResponse> SendMessageToWhatsApp(MessageRequest request);

        Task<SimpleResponse<ResponseMessage>> UploadToWhatsappMediaAsync(MessageRequest request);
    }
}
