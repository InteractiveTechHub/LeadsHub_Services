using WhatsApp.Core.Models.Error;
using WhatsApp.Core.Request;
using WhatsApp.Core.Interfaces.IServices;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using WhatsApp.Core.Response;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace WhatsApp.Core.Services
{
    public sealed class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BaseService> _logger;

        public BaseService(IHttpClientFactory httpClientFactory, ILogger<BaseService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<JsonResponse> SendMessageAsync(MessageRequest request)
        {
            JsonResponse response = new();

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("WhatsappApi");

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
                _logger.LogError($"::LeadsManager.Whatsapp.Api.{MethodBase.GetCurrentMethod()} ::{ex.Message}");
                response.AddExceptionMessage(ex.Message);
            }

            return response;
        }

        public async Task<JsonResponse> GetAsync(MessageRequest request)
        {
            JsonResponse response = new();

            try
            {
                HttpClient client = _httpClientFactory.CreateClient("WhatsappApi");

                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                if (!string.IsNullOrWhiteSpace(request.AccessToken))
                {
                    message.Headers.Add("Authorization", $"Bearer {request.AccessToken}");
                }

                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri($"{request.Url}");

                //message.Content = new StringContent(request.DataJson, Encoding.UTF8, "application/json");

                HttpResponseMessage apiResponse = await client.SendAsync(message);

                response = await GetDefaultResponse(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"::LeadsManager.Whatsapp.Api.{MethodBase.GetCurrentMethod()} ::{ex.Message}");
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
                HttpStatusCode.InternalServerError => "Whataspp server internal error",
                HttpStatusCode.ServiceUnavailable => "Whatsapp Api unavailable",
                _ => string.Empty,
            };
            
            if (!string.IsNullOrWhiteSpace(serverError))
            {
                response.AddErrorMessage(serverError);

                return response;
            }

            response.DataJson = await apiResponse.Content.ReadAsStringAsync();        

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            ErrorPayLoad? errorPayLoad = JsonSerializer.Deserialize<ErrorPayLoad>(response.DataJson, options);

            if (errorPayLoad?.Error.Code >  0)
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
