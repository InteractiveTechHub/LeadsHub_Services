
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Enum;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads;
using LeadsHub.Core.Payloads.Whatsapp.Response;
using LeadsHub.Core.Request;
using LeadsHub.Core.Responses;
using LeadsHub.Core.Services;
using LeadsHub.Core.Utility;
using Microsoft.AspNetCore.Http;

namespace LeadsHub.Core.Bac
{
    public class WhatsAppBac : IWhatsAppBac
    {
        private readonly ILeadBrokerBac _leadBrokerBac;
        private readonly IWhatsappService _whatsappService;
        private readonly IWhatsAppRepository _whatsAppRepository;

        public WhatsAppBac(ILeadBrokerBac leadBrokerBac, IWhatsAppRepository whatsAppRepository, IWhatsappService whatsappService)
        {
            _leadBrokerBac = leadBrokerBac;
            _whatsAppRepository = whatsAppRepository;
            _whatsappService = whatsappService;
        }

        public async Task ReceiveMessageFromWhatsappAsync(WhatsAppPayLoad whatsappMessage)
        {
            foreach (var entry in whatsappMessage.Entry)
            {
                foreach (PayLoadChange change in entry.Changes)
                {   
                    if (change.Value.Messages.Any())
                    {
                        await BuildLeadAndMessage(change);
                    }                   
                }
            }
        }

        private async Task BuildLeadAndMessage(PayLoadChange change)
        {
            Lead lead = new();
            string phoneNumberId = change.Value.Metadata.PhoneNumberId;

            FilterRequest filter = new();
            filter.AddFilter(nameof(WhatsAppConfig.PhoneNumberId), FilterOperatorEnum.Equals, phoneNumberId, "w");

            BaseResponse<Integration> response = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filter);
            if (response.HasAnyErrorMessage)
            {
                return;
            }

            if (response.ResponseData is null || !response.ResponseData.Any())
            {
                return;
            }

            lead.CompanyId = response.ResponseData.Select(r => r.CompanyId).FirstOrDefault();
            lead.Contact.Name = change.Value.Contacts.Select(c => c.Profile.Name).FirstOrDefault() ?? string.Empty;
            lead.IntegrationId = response.ResponseData.Select(r => r.Id).FirstOrDefault();
            lead.Channel = 1; //"Whatsapp";

            foreach (PayLoadMessage message in change.Value.Messages)
            {
                Timeline timeline = new();

                lead.Contact.PhoneNumber = $"+{message.From}";
                timeline.MessageId = message.Id;
                timeline.Status = MessageStatus.Delivered;

                timeline.ConvertsTimeUnixToUtcDateTime(message.TimeStamp);

                if (message.Type.Equals("text"))
                {
                    timeline.Type = MessageType.Text;
                    timeline.Message = new()
                    {
                        Body = message.Text.Body
                    };
                }

                //TODO: Implement others feature later.
                if (message.Type.Equals("reaction"))
                {
                    timeline.Type = MessageType.Reaction;
                    timeline.MessageReaction!.Emoji = message.Reaction.Emoji;
                    timeline.MessageReaction!.MessageId = message.Reaction.MessageId;
                }

                if (message.Type.Equals("image") || message.Type.Equals("video") || message.Type.Equals("audio") || message.Type.Equals("document"))
                {
                    WhatsAppConfig? config = response.ResponseData.Select(r => r.WhatsAppConfig).FirstOrDefault();
                    if (config is null)
                    {
                        return;
                    }

                    string messageId = DetectFileId(message);

                    var uri = await _whatsappService.GetMediaFromWhatsapp(messageId, config);
                    if (string.IsNullOrEmpty(uri))
                    {
                        return;
                    }

                    timeline.Type = DetectMessageType(message.Type);
                    timeline.MessageFile = DetectMessageFile(message);
                    timeline.MessageFile.Url = $"{SD.S3BaseUrl}/{uri}";
                    string whatsImageId = message.Image.Sha256;
                }

                if (message.Type.Equals("unsupported"))
                {
                    return;
                }

                lead.Timelines.Add(timeline);
            }

            await _leadBrokerBac.ReceiveLeadsAsync(lead);
        }

        public async Task<BaseResponse<Integration>> FetchAllWhatsAppByRequestAsync(FilterRequest filterRequest)
        {
            var response = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filterRequest);

            return response;
        }

        private MessageFile DetectMessageFile(PayLoadMessage message)
        {
            string whatsType = message.Type.ToLower();

            return whatsType switch
            {
                string type when type.Equals("image") => new MessageFile() { Caption = message.Image.Caption, MimeType = message.Image.MimeType },
                string type when type.Equals("video") => new MessageFile() { Caption = message.Video.Caption, MimeType = message.Video.MimeType },
                string type when type.Equals("audio") => new MessageFile() { MimeType = message.Audio.MimeType },
                string type when type.Equals("document") => new MessageFile() { Caption = message.Document.Caption, MimeType = message.Document.MimeType },
            };
        }

        private MessageType DetectMessageType(string whatsType)
        {
            whatsType = whatsType.ToLower();

            return whatsType switch
            {
                string type when type.Equals("image") => MessageType.Image,
                string type when type.Equals("video") => MessageType.Video,
                string type when type.Equals("audio") => MessageType.Audio,
                string type when type.Equals("document") => MessageType.Document,
                _ => MessageType.Document // fallback
            };
        }
        
        private string DetectFileId(PayLoadMessage message)
        {
            return message.Type switch
            {
                string type when type.Equals("image") => message.Image.Id,
                string type when type.Equals("video") => message.Video.Id,
                string type when type.Equals("audio") => message.Audio.Id,
                string type when type.Equals("document") => message.Document.Id,
                _ => message.Document.Id // fallback
            };
        }
    }
}
