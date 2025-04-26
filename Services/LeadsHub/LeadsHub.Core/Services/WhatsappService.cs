
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads.Whatsapp.Error;
using LeadsHub.Core.Payloads.Whatsapp.Response;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Utility;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;


namespace LeadsHub.Core.Services
{
    public sealed class WhatsappService(IHttpClientFactory httpClientFactory, AmazonS3Service amazonS3Service) : BaseHttpService(httpClientFactory), IWhatsappService
    {
        private readonly AmazonS3Service _amazonS3Service = amazonS3Service;

        public async Task<string> GetMediaFromWhatsapp(string mediaId, WhatsAppConfig config)
        {
            MessageRequest request = new()
            {
                AccessToken = config.AccessToken,
                Url = $"{SD.WhatsAppAPIBase}/{mediaId}"
            };

            // 1 - Get the media info from whatsapp
            var httpResponse = await GetAsync(request);
            if (!httpResponse.IsSuccessStatusCode)
            {
                var r = GetDefaultResponse(httpResponse);

                return string.Empty;
            }        

            MediaResponse? media = await httpResponse.Content.ReadFromJsonAsync<MediaResponse>();
            if (media is null)
            {
                return string.Empty;
            }

            // 2 - Download the media from whatsapp
            request.Url = media.Url;

            httpResponse = await GetAsync(request);
            if (!httpResponse.IsSuccessStatusCode)
            {
                var r = GetDefaultResponse(httpResponse);
            }

            Stream fileStream = await httpResponse.Content.ReadAsStreamAsync();

            // 3 - Turn into IFormFile and then upload to S3
            string key = $"whatsap/{media.MimeType}/{mediaId}-{Guid.NewGuid()}";

            bool isUpdload = await _amazonS3Service.UploadFileAsync(key, fileStream, media.MimeType);
            if (!isUpdload)
            {
                //TODO: log something here maybe
                return string.Empty;
            }

            return key;
        }

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

        /// <summary>
        /// Get default response object
        /// </summary>
        /// <param name="apiResponse">Http response message</param>
        /// <returns>default response</returns>
        private static async Task<BaseResponse> GetDefaultResponse(HttpResponseMessage apiResponse)
        {
            BaseResponse response = new();

            string serverError = apiResponse.StatusCode switch
            {
                HttpStatusCode.Forbidden => "Permission denied",
                HttpStatusCode.InternalServerError => "Server Internal Error",
                HttpStatusCode.ServiceUnavailable => "Service Unavailable",
                _ => string.Empty,
            };

            if (!string.IsNullOrWhiteSpace(serverError))
            {
                response.AddErrorMessage(serverError);

                return response;
            }

            return response;
        }

        private string GetExtensionFromMimeType(string mimeType)
        {
            return mimeType switch
            {
                "image/jpeg" => "jpg",
                "image/png" => "png",
                "image/webp" => "webp",
                "video/mp4" => "mp4",
                "audio/ogg" => "ogg",
                _ => "bin"
            };
        }
    }
}
