
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads.Whatsapp.SendMessage;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Utility;
using System.Text.Json;

namespace LeadsHub.Core.Bac
{
    public sealed class TimelineBac : ITimelineBac
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ISendMessageService _sendMessageService;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IWhatsAppRepository _whatsAppRepository;

        public TimelineBac(ILeadRepository leadRepository, ISendMessageService sendMessageService, ITimelineRepository timelineRepository, IWhatsAppRepository whatsAppRepository)
        {
            _leadRepository = leadRepository;
            _sendMessageService = sendMessageService;
            _timelineRepository = timelineRepository;
            _whatsAppRepository = whatsAppRepository;
        }

        public async Task<TimelineResponse> FetchTimelineByRequestAsync(long leadId, FilterRequest filterRequest)
        {
            TimelineResponse response = await _timelineRepository.FetchTimelineByRequestAsync(leadId, filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return response;
            }

            return response;
        }

        public async Task<SimpleResponse<Timeline>> RegisterTimelineAsync(Timeline timeline)
        {
            SimpleResponse<Timeline> response = new();

            SimpleResponse<Lead?> leadResponse = await _leadRepository.FetchLeadByIdAsync(timeline.LeadId);
            if (leadResponse.HasAnyErrorMessage)
            {
                return response;
            }

            SendMessagePayLoad sendMessagePayLoad = new()
            {
                RecepientType = "individual",
                To = leadResponse.Model!.Contact.PhoneNumber,
            };

            // text
            if (timeline.Type == 1)
            {
                sendMessagePayLoad.Type = "text";
                sendMessagePayLoad.Text = new()
                {
                    PreviewUrl = false,
                    Body = timeline.Message!.Body
                };

                response = await _timelineRepository.RegisterMessageTextAsync(timeline);              
            }

            // template
            if (timeline.Type == 2)
            {
                sendMessagePayLoad.Template = new()
                {
                    Name = "",
                    Language = new()
                    {
                        Code = "pt_BR",
                    }
                };
            }

            // file
            if (timeline.Type == 3)
            {

            }

            // reaction
            if (timeline.Type == 4)
            {

            }

            await SendTextMessageAsync(sendMessagePayLoad, leadResponse.Model.IntegrationId);

            return response;
        }

        /// <summary>
        /// The only responsability of this method is send message of kind text
        /// </summary>
        /// <param name="timeline">Message to send</param>
        /// <returns></returns>
        private async Task SendTextMessageAsync(SendMessagePayLoad sendMessagePayLoad, long integrationId)
        {           
            // Buscar lead para pegar integração.

            FilterRequest filter = new();
            filter.AddFilter(nameof(Integration.Id), FilterOperatorEnum.Equals, integrationId, "i");

            var response = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filter);
            if (response.HasAnyErrorMessage)
            {
                //should return with some error.
                //return new();
            }

            MessageRequest request = new();

            request.AccessToken = response.ResponseData.Select(r => r.WhatsAppConfig!.AccessToken).First();
            string phoneNumberId = response.ResponseData.Select(r => r.WhatsAppConfig!.PhoneNumberId).First();

            request.Url = SD.WhatsAppAPIBase + $"/{phoneNumberId}/messages";
            request.DataJson = JsonSerializer.Serialize(sendMessagePayLoad);

            var sendResponse = await _sendMessageService.SendMessageToWhatsApp(request);
        }
    }
}
