
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Payloads.Whatsapp.Error;
using LeadsHub.Core.Payloads.Whatsapp.Response;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using System.Text.Json;


namespace LeadsHub.Core.Services
{
    public sealed class WhatsappService(IHttpClientFactory httpClientFactory) : BaseHttpService(httpClientFactory), IWhatsappService
    {
        public async Task<BaseResponse> SendMessageToWhatsApp(MessageRequest request)
        {
            JsonResponse response = await PostAsync(request);
            if (response.HasAnyErrorMessage)
            {
                return response;
            }

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

            WhatsappResponse? whatsResponse = JsonSerializer.Deserialize<WhatsappResponse>(response.DataJson, options);
            if (whatsResponse is not null)
            {
                return response;
            }

            return response;
        }

        public async Task<SimpleResponse<ResponseMessage>> UploadToWhatsappMediaAsync(MessageRequest request)
        {
            SimpleResponse<ResponseMessage> response = new();

            JsonResponse result = await PostFileAsync(request);
            if (result.HasAnyErrorMessage)
            {
                response.Messages = result.Messages;
                return response;
            }

            // Verify if there are any error in the response.
            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            ErrorPayLoad? errorPayLoad = JsonSerializer.Deserialize<ErrorPayLoad>(result.DataJson, options);

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

            ResponseMessage? whatsResponse = JsonSerializer.Deserialize<ResponseMessage>(result.DataJson, options);
            if (whatsResponse is not null)
            {
                response.Model = whatsResponse;

                return response;
            }

            return response;          
        }
    }
}
