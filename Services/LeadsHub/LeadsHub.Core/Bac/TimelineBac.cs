
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Enum;
using LeadsHub.Core.Extensions;
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
        private readonly IActiveChatManager _activeChatManager;
        private readonly ILeadRepository _leadRepository;
        private readonly ISendMessageService _sendMessageService;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IWhatsAppRepository _whatsAppRepository;

        public TimelineBac(IActiveChatManager activeChatManager, 
            ILeadRepository leadRepository, 
            ISendMessageService sendMessageService, 
            ITimelineRepository timelineRepository, 
            IWhatsAppRepository whatsAppRepository)
        {
            _activeChatManager = activeChatManager;
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

            DateTimeOffset lastMessageDateTime = response.ResponseData.Select(r => r.MessageDate).Last();

            _activeChatManager.AddLead(leadId, lastMessageDateTime);
            response.CanSendMessage = _activeChatManager.CanSendFreeMessage(leadId);
            // TODO: Fetch the templates to send;

            return response;
        }

        /// <summary>
        /// Register timiline, messages, files and etc..
        /// </summary>
        /// <param name="timeline">Timeline containing it's childrens</param>
        /// <returns>Response of operation</returns>
        public async Task<SimpleResponse<Timeline>> RegisterTimelineAsync(Timeline timeline)
        {
            SimpleResponse<Timeline> response = new();

            timeline.Sender = MessageSender.consultant;
            timeline.Status = MessageStatus.Sent;

            SimpleResponse<Lead?> leadResponse = await _leadRepository.FetchLeadByIdAsync(timeline.LeadId);
            if (leadResponse.HasAnyErrorMessage)
            {
                response.Messages.AddRange(leadResponse.Messages);
                return response;
            }

            if (leadResponse.Model is null || leadResponse.Model.Id == 0)
            {
                response.AddErrorMessage("Lead not found");
                return response;
            }

            // TODO: The only person who can send a message to the lead is the one assigned to it
            // TODO: If lead does not have a consultant, it should not be possible to send a message

            SendMessagePayLoad sendMessagePayLoad = new()
            {
                RecepientType = "individual",
                To = leadResponse.Model!.Contact.PhoneNumber.RemovePhoneFormat(),
            };

            // text
            if (timeline.Type == MessageType.Text)
            {
                sendMessagePayLoad.Type = "text";
                sendMessagePayLoad.Text = new()
                {
                    PreviewUrl = false,
                    Body = timeline.Message!.Body
                };            
            }

            // template
            if (timeline.Type == MessageType.Template)
            {
                var templateResponse = await _whatsAppRepository.FetchWhatsAppTemplateByIdAsync(timeline.TemplateId!.Value);
                if (templateResponse.HasAnyErrorMessage)
                {
                    response.Messages.AddRange(templateResponse.Messages);
                    return response;
                }

                WhatsAppTemplate template = templateResponse.Model;

                sendMessagePayLoad.Template = new()
                {
                    Name = template.Name,
                    Language = new()
                    {
                        Code = template.Language,
                    }
                };
            }

            response = await _timelineRepository.RegisterMessageTextAsync(timeline);
            if (response.HasAnyErrorMessage)
            {
                return response;
            }

            var result = await SendTextMessageAsync(sendMessagePayLoad, leadResponse.Model.IntegrationId);
            if (result.HasAnyErrorMessage)
            {
                response.Messages.AddRange(result.Messages);
                response.Model.Status = MessageStatus.Failed;

                await _timelineRepository.UpdateTimelineAsync(response.Model);

                // TODO: Should log the whatsapp exceptions and errors.
            }

            // If it is new and message was sent and lead is new.
            if (!result.HasAnyErrorMessage && leadResponse.Model.Phase.Equals(LeadPhase.New))
            {
                leadResponse.Model.Phase = LeadPhase.InProgress;

                await _leadRepository.UpdateLeadAsync(leadResponse.Model);
            }

            return response;
        }

        /// <summary>
        /// The only responsability of this method is send message of kind text
        /// </summary>
        /// <param name="timeline">Message to send</param>
        /// <returns></returns>
        private async Task<BaseResponse> SendTextMessageAsync(SendMessagePayLoad sendMessagePayLoad, long integrationId)
        {
            BaseResponse response = new();

            FilterRequest filter = new();
            filter.AddFilter(nameof(Integration.Id), FilterOperatorEnum.Equals, integrationId, "i");

            var configRespose = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filter);
            if (response.HasAnyErrorMessage)
            {
                //should return with some error.
                response.Messages.AddRange(configRespose.Messages);
                
                return response;
            }

            MessageRequest request = new();

            request.AccessToken = configRespose.ResponseData.Select(r => r.WhatsAppConfig!.AccessToken).First();
            string phoneNumberId = configRespose.ResponseData.Select(r => r.WhatsAppConfig!.PhoneNumberId).First();

            request.Url = SD.WhatsAppAPIBase + $"/{phoneNumberId}/messages";
            request.DataJson = JsonSerializer.Serialize(sendMessagePayLoad);

            response = await _sendMessageService.SendMessageToWhatsApp(request);

            return response;
        }
    }
}
