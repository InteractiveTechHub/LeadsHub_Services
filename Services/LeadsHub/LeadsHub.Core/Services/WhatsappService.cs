
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Payloads.Whatsapp.Error;
using LeadsHub.Core.Payloads.Whatsapp.Response;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using System.Net.Http.Headers;
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

        public async Task<string> UploadToWhatsappMediaAsync(MessageRequest request)
        {
            using var memoryStream = new MemoryStream();
            await memoryStream.CopyToAsync(memoryStream);
            byte[] fileBytes = memoryStream.ToArray();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);

            var content = new MultipartFormDataContent();
            var byteArrayContent = new ByteArrayContent(fileBytes);
            byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(request.FormFile.ContentType);

            var mediaType = GetWhatsappMediaType(request.FormFile.ContentType);
            content.Add(byteArrayContent, "file", request.FormFile.FileName);
            content.Add(new StringContent(mediaType), "type"); // pode ser "audio", "video"

            var response = await client.PostAsync(request.Url, content);

            var responseString = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao enviar mídia: {responseString}");

            var json = JsonDocument.Parse(responseString);
            return json.RootElement.GetProperty("id").GetString() ?? "";
        }

        private string GetWhatsappMediaType(string contentType)
        {
            if (contentType.StartsWith("image/"))
                return "image";
            if (contentType.StartsWith("audio/"))
                return "audio";
            if (contentType.StartsWith("video/"))
                return "video";

            // WhatsApp aceita "document" como catch-all para outros tipos (ex: PDF, DOCX, etc.)
            return "document";
        }
    }
}
