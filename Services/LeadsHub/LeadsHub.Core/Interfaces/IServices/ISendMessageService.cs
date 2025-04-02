
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Request;

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface ISendMessageService
    {
        //Task GetTemplatesFromWhatsApp(MessageRequest request);

        Task<BaseResponse> SendMessageToWhatsApp(MessageRequest request);
    }
}
