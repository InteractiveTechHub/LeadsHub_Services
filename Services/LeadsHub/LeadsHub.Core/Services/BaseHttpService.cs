
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Services
{
    public class BaseHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<JsonResponse> PostAsync(MessageRequest request)
        {
            JsonResponse response = new();

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("leadsManager");

                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                if (!string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    message.Headers.Add("Authorization", $"Bearer {request.AccessToken}");
                }

                message.Method = HttpMethod.Post;
                message.RequestUri = new Uri($"{request.Url}");
                message.Content = new StringContent(request.DataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                response = await GetDefaultResponse(apiResponse);
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<JsonResponse> GetAsync(MessageRequest request)
        {
            JsonResponse response = new();

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("leadsManager");

                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                if (!string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    message.Headers.Add("Authorization", $"Bearer {request.AccessToken}");
                }

                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri($"{request.Url}");
                message.Content = new StringContent(request.DataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                response = await GetDefaultResponse(apiResponse);
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<JsonResponse> PostFileAsync(MessageRequest request)
        {
            JsonResponse response = new();

            try
            {
                using var memoryStream = new MemoryStream();

                await request.FormFile.CopyToAsync(memoryStream);
                byte[] fileBytes = memoryStream.ToArray();

                HttpClient client = _httpClientFactory.CreateClient("leadsManager");

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.AccessToken);

                var content = new MultipartFormDataContent();
                var byteArrayContent = new ByteArrayContent(fileBytes);
                byteArrayContent.Headers.ContentType = new MediaTypeHeaderValue(request.FormFile.ContentType);

                var mediaType = GetWhatsappMediaType(request.FormFile.ContentType);

                content.Add(new StringContent("whatsapp"), "messaging_product");
                content.Add(byteArrayContent, "file", request.FormFile.FileName);
                content.Add(new StringContent(mediaType), "type");

                HttpResponseMessage apiResponse = await client.PostAsync(request.Url, content);

                response = await GetDefaultResponse(apiResponse);
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        /// <summary>
        /// Get default response object
        /// </summary>
        /// <param name="apiResponse">Http response message</param>
        /// <returns>default response</returns>
        private static async Task<JsonResponse> GetDefaultResponse(HttpResponseMessage apiResponse)
        {
            JsonResponse response = new();

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

            response.DataJson = await apiResponse.Content.ReadAsStringAsync();   

            return response;
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
