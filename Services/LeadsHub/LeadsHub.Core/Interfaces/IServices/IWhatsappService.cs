
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Request;

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface IWhatsappService
    {
        //Task GetTemplatesFromWhatsApp(MessageRequest request);

        Task<BaseResponse> SendMessageToWhatsApp(MessageRequest request);

        Task<string> UploadToWhatsappMediaAsync(MessageRequest request);
    }
}
