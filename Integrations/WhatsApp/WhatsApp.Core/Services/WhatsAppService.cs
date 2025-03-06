using WhatsApp.Core.Interfaces.IBac;
using WhatsApp.Core.Interfaces.IServices;
using WhatsApp.Core.Models.Send;
using WhatsApp.Core.Request;
using WhatsApp.Core.Utility;
using System.Text.Json;
using CrossCutting.Models;
using WhatsApp.Core.Response;
using AdaptiveKitCore.Requests;
using WhatsApp.Core.Models;
using AdaptiveKitCore.Enums;

namespace WhatsApp.Core.Services
{
    public sealed class WhatsappService : IWhatsAppService
    {
        private readonly IBaseService _baseService;
        private readonly IWhatsAppConfigBac _whatsAppConfigBac;

        public WhatsappService(IBaseService baseService, 
            IWhatsAppConfigBac whatsAppConfigBac)
        {
            _baseService = baseService;
            _whatsAppConfigBac = whatsAppConfigBac;
        }

        public async Task<JsonResponse> SendMessageToWhatsappAsync(TransferLead transferLead)
        {
            MessageRequest request = new();            

            SendMessagePayLoad sendMessagePayLoad = new()
            {
                RecepientType = "individual",
                To = transferLead.PhoneNumber,
                Type = "text" //transferLead.MessageType,
            };

            if (transferLead.MessageType == 1)
            {
                sendMessagePayLoad.Text = new()
                {
                    PreviewUrl = false,
                    Body = transferLead.MessageBody
                };
            }

            /*if (transferLead.MessageType.Equals("Template"))
            {
                sendMessagePayLoad.Template = new()
                {
                    Name = transferLead.TemplateName,
                    Language = new()
                    {
                        Code = "pt_BR",
                    }
                };
            }*/

            FilterRequest filter = new();
            filter.AddFilter(nameof(Integration.Id), FilterOperatorEnum.Equals, transferLead.IntegrationId, "i");

            var response = await _whatsAppConfigBac.FetchWhatsappConfigByRequestAsync(filter);
            if (response.HasAnyErrorMessage)
            {
                //should return with some error.
                return new();
            }

            request.AccessToken = response.ResponseData.Select(r => r.WhatsappConfig!.AccessToken).First();
            string phoneNumberId = response.ResponseData.Select(r => r.WhatsappConfig!.PhoneNumberId).First();

            request.Url = SD.WhatsappAPIBase + $"/{phoneNumberId}/messages";
            request.DataJson = JsonSerializer.Serialize(sendMessagePayLoad);

            JsonResponse jsonResponse = await _baseService.SendMessageAsync(request);
            // Should app whatsapp errors.

            return jsonResponse;
        }
    }
}
