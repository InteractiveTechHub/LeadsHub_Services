
using AdaptiveKitCore.Responses;
using CrossCutting.Models;

namespace LeadsHub.Core.Interfaces.IServices
{
    public interface ISendMessageService
    {
        Task<BaseResponse> SendMessageToWhatsApp(TransferLead transferLead);
    }
}
