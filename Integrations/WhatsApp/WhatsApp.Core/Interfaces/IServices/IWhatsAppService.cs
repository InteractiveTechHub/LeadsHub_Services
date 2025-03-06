
using CrossCutting.Models;
using WhatsApp.Core.Response;

namespace WhatsApp.Core.Interfaces.IServices
{
    public interface IWhatsAppService
    {
        /// <summary>
        /// Send message to whatsapp number
        /// </summary>
        /// <returns></returns>
        Task<JsonResponse> SendMessageToWhatsappAsync(TransferLead request);
    }
}
