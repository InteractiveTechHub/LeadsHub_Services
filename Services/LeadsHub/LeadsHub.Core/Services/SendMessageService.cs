
using AdaptiveKitCore.Responses;
using CrossCutting.Models;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Payloads.Whatsapp.Error;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using System.Text.Json;


namespace LeadsHub.Core.Services
{
    public sealed class SendMessageService(IHttpClientFactory httpClientFactory) : BaseHttpService(httpClientFactory), ISendMessageService
    {
        public async Task<BaseResponse> SendMessageToWhatsApp(MessageRequest request)
        {
            JsonResponse response = await PostAsync(request);

            // Verify if there are any error in the response.
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            ErrorPayLoad? errorPayLoad = JsonSerializer.Deserialize<ErrorPayLoad>(response.DataJson, options);

            if (errorPayLoad?.Error.Code > 0)
            {
                string errorDetails = errorPayLoad.Error.ErrorData.Details;

                // If there is error and no details.
                if (string.IsNullOrWhiteSpace(errorDetails))
                {
                    response.AddErrorMessage("Error while trying to send message by whatsapp. Please contact the support");

                    return response;
                }

                response.AddErrorMessage(errorDetails);
            }

            return response;
        }
    }
}
