
using WhatsApp.Core.Response;
using WhatsApp.Core.Request;

namespace WhatsApp.Core.Interfaces.IServices
{
    public interface IBaseService
    {
        Task<JsonResponse> GetAsync(MessageRequest request);

        /// <summary>
        /// Send Message to any intern or extern api
        /// </summary>
        /// <param name="request">Request containing the messages and parameters</param>
        /// <returns></returns>
        Task<JsonResponse> SendMessageAsync(MessageRequest request);
    }
}
