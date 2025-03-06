
using System.Net;
using System.Text.Json;
using System.Text;
using AdaptiveKitCore.Responses;

namespace LeadsHub.Core.Services
{
    public class BaseHttpService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseHttpService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<BaseResponse> SendMessageAsync(string uri, string messageSerielized)
        {
            BaseResponse response = new();

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("leadsManager");

                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //message.Headers.Add("Authorization", $"Bearer {TempToken}");
                message.Method = HttpMethod.Post;

                message.RequestUri = new Uri("http://whatsapp_api:8080/api/v1/whatsapp/sendmessage");

                message.Content = new StringContent(messageSerielized, Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                HttpStatusCode statusCode = apiResponse.StatusCode;
                string apiContent = await apiResponse.Content.ReadAsStringAsync();

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                response = JsonSerializer.Deserialize<BaseResponse>(apiContent, options) ?? new();
            }
            catch (Exception ex)
            {
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }
    }
}
