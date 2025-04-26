
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using Amazon.RuntimeDependencies;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Enum;
using LeadsHub.Core.Extensions;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads.Whatsapp.Response;
using LeadsHub.Core.Payloads.Whatsapp.SendMessage;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Services;
using LeadsHub.Core.Utility;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace LeadsHub.Core.Bac
{
    public sealed class TimelineBac : ITimelineBac
    {
        private readonly IActiveChatManager _activeChatManager;
        private readonly ILeadRepository _leadRepository;
        private readonly IWhatsappService _whatsappService;
        private readonly ITimelineRepository _timelineRepository;
        private readonly IWhatsAppRepository _whatsAppRepository;

        private readonly AmazonS3Service _amazonS3;

        public TimelineBac(
            AmazonS3Service amazonS3,
            IActiveChatManager activeChatManager, 
            ILeadRepository leadRepository, 
            IWhatsappService whatsappService, 
            ITimelineRepository timelineRepository, 
            IWhatsAppRepository whatsAppRepository)
        {
            _amazonS3 = amazonS3;
            _activeChatManager = activeChatManager;
            _leadRepository = leadRepository;
            _whatsappService = whatsappService;
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
        public async Task<SimpleResponse<Timeline>> RegisterTimelineAsync(TimelineFormData formData)
        {
            SimpleResponse<Timeline> response = new();

            Timeline timeline = formData.Timeline;
            Lead lead = formData.Lead;

            timeline.LeadId = lead.Id;
            timeline.Sender = MessageSender.consultant;
            timeline.Status = MessageStatus.Sent;

            // TODO: The only person who can send a message to the lead is the one assigned to it
            // TODO: If lead does not have a consultant, it should not be possible to send a message

            SendMessagePayLoad sendMessagePayLoad = new()
            {
                RecepientType = "individual",
                To = lead.Contact.PhoneNumber.RemovePhoneFormat(),
            };

            if (!timeline.IsFile)
            {
                response = await SendTextMessageAsync(timeline, sendMessagePayLoad, lead.IntegrationId);
            }
            
            if (timeline.IsFile && timeline.MessageFile is not null)
            {
                formData.Timeline = timeline;

                response = await UploadFileToWhatsappAsync(formData, sendMessagePayLoad, lead.IntegrationId);              
            }

            if (response.HasAnyErrorMessage)
            {
                response.Model.Status = MessageStatus.Failed;

                await _timelineRepository.UpdateTimelineAsync(response.Model);
            }

            // If it is new and message was sent and lead is new.
            if (!response.HasAnyErrorMessage && lead.Phase.Equals(LeadPhase.New))
            {
                lead.Phase = LeadPhase.InProgress;

                await _leadRepository.UpdateLeadAsync(lead);
            }

            return response;
        }

        /// <summary>
        /// The only responsability of this method is send message of kind text to whatsapp
        /// </summary>
        /// <param name="timeline">The timeline to be registered</param>
        /// <param name="sendMessagePayLoad">The whatsapp object request</param>
        /// <param name="integrationId">Id of the integration that the lead is assigned</param>
        /// <returns>A default response</returns>
        private async Task<SimpleResponse<Timeline>> SendTextMessageAsync(Timeline timeline, SendMessagePayLoad sendMessagePayLoad, long integrationId)
        {
            SimpleResponse<Timeline> response = new();

            // text
            if (timeline.Type.Equals(MessageType.Text))
            {
                sendMessagePayLoad.Type = "text";
                sendMessagePayLoad.Text = new()
                {
                    PreviewUrl = false,
                    Body = timeline.Message!.Body
                };
            }

            // template
            if (timeline.Type.Equals(MessageType.Template))
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
            request.AccessToken = configRespose.ResponseData.Select(r => r.WhatsAppConfig!.AccessToken).FirstOrDefault() ?? string.Empty;
            string phoneNumberId = configRespose.ResponseData.Select(r => r.WhatsAppConfig!.PhoneNumberId).FirstOrDefault() ?? string.Empty;

            request.Url = SD.WhatsAppAPIBase + $"/{phoneNumberId}/messages";
            request.DataJson = JsonSerializer.Serialize(sendMessagePayLoad);

            var baseResponse = await _whatsappService.SendMessageToWhatsApp(request);
            if (baseResponse.HasAnyErrorMessage)
            {
                response.Messages.AddRange(baseResponse.Messages);
                return response;
            }

            return response;
        }

        /// <summary>
        /// Upload file to whatsapp, store the file in amazon s3 and the timeline
        /// </summary>
        /// <param name="timeline">Timeline containning data to be registered</param>
        /// <param name="formFile">The files</param>
        /// <param name="sendMessagePayLoad">The payload request of whatsapp</param>
        /// <param name="integrationId">Id of the integration that the lead is assigned</param>
        /// <returns>Default response</returns>
        private async Task<SimpleResponse<Timeline>> UploadFileToWhatsappAsync(TimelineFormData formData, SendMessagePayLoad sendMessagePayLoad, long integrationId)
        {
            SimpleResponse<Timeline> response = new();
            MessageRequest request = new();

            Timeline timeline = formData.Timeline;
            IFormFile formFile = formData.FormFile!;    

            // 1 - Fetch whatsapp configuration
            FilterRequest filter = new();
            filter.AddFilter(nameof(Integration.Id), FilterOperatorEnum.Equals, integrationId, "i");

            var configRespose = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filter);
            if (response.HasAnyErrorMessage)
            {
                //should return with some error.
                // TODO: Should return with some error.
                response.Messages.AddRange(configRespose.Messages);

                return response;
            }

            request.AccessToken = configRespose.ResponseData.Select(r => r.WhatsAppConfig!.AccessToken).FirstOrDefault() ?? string.Empty;
            string phoneNumberId = configRespose.ResponseData.Select(r => r.WhatsAppConfig!.PhoneNumberId).FirstOrDefault() ?? string.Empty;

            request.Url = SD.WhatsAppAPIBase + $"/{phoneNumberId}/media";
            request.FormFile = formData.FormFile!;          

            // 2 -  Upload to whatsapp and returns image Id from whatsapp
            SimpleResponse<ResponseMessage> uploadResponse = await _whatsappService.UploadToWhatsappMediaAsync(request);
            if (uploadResponse.HasAnyErrorMessage)
            {
                response.Messages.AddRange(uploadResponse.Messages);
                return response;
            }

            // 3 - Upload to AWS S3
            string fileId = uploadResponse.Model.Id;
            string key = $"{timeline.MessageFile!.MimeType}/{fileId}-" + Guid.NewGuid();    

            bool isUplodated = await _amazonS3.UploadFileAsync(key, formFile);
            if (!isUplodated)
            {
                response.AddErrorMessage("File not uploaded to S3");
                return response;
            }

            // 4 -- Register timeline with file message
            timeline.MessageFile!.Url = $"{SD.S3BaseUrl}/{key}";

            response = await _timelineRepository.RegisterMessageFileAsync(timeline);
            if (response.HasAnyErrorMessage)
            {
                return response;
            }

            timeline = response.Model; //timeline updated with the Id
            
            // Send message with the file
            if (timeline.Type.Equals(MessageType.Image))
            {
                sendMessagePayLoad.Type = "image";
                sendMessagePayLoad.Image = new()
                {
                    Id = fileId,
                    Caption = timeline.MessageFile?.Caption ?? string.Empty,
                };
            }

            if (timeline.Type.Equals(MessageType.Video))
            {
                sendMessagePayLoad.Type = "video";
                sendMessagePayLoad.Video = new()
                {
                    Id = fileId,
                    Caption = timeline.MessageFile?.Caption ?? string.Empty,
                };
            }

            if (timeline.Type.Equals(MessageType.Audio))
            {
                sendMessagePayLoad.Type = "audio";
                sendMessagePayLoad.Audio = new()
                {
                    Id = fileId
                };
            }

            request.DataJson = JsonSerializer.Serialize(sendMessagePayLoad);
    
            request.Url = SD.WhatsAppAPIBase + $"/{phoneNumberId}/messages";

            var baseResponse = await _whatsappService.SendMessageToWhatsApp(request);
            if (baseResponse.HasAnyErrorMessage)
            {
                response.Messages.AddRange(baseResponse.Messages);
                return response;
            }

            return response;
        }
    }
}
