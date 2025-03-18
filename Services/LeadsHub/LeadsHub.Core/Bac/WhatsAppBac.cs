
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Payloads;

namespace LeadsHub.Core.Bac
{
    public class WhatsAppBac : IWhatsAppBac
    {
        private ILeadBrokerBac _leadBrokerBac;
        private readonly IWhatsAppRepository _whatsAppRepository;

        public WhatsAppBac(ILeadBrokerBac leadBrokerBac, IWhatsAppRepository whatsAppRepository)
        {
            _leadBrokerBac = leadBrokerBac;
            _whatsAppRepository = whatsAppRepository;
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

            var response = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filter);
            if (response.HasAnyErrorMessage)
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

                lead.Contact.PhoneNumber = message.From;
                timeline.MessageId = message.Id;
                timeline.Sender = 1; //lead
                timeline.Status = 1; //Pending

                timeline.ConvertsTimeUnixToUtcDateTime(message.TimeStamp);

                if (message.Type.Equals("text"))
                {
                    timeline.Type = 1;
                    timeline.Message = new()
                    {
                        Body = message.Text.Body
                    };
                }

                //TODO: Implement others feature later.
                if (message.Type.Equals("reaction"))
                {
                    timeline.Type = 2;
                    timeline.MessageReaction!.Emoji = message.Reaction.Emoji;
                    timeline.MessageReaction!.MessageId = message.Reaction.MessageId;
                }

                if (message.Type.Equals("image"))
                {
                    timeline.Type = 3;
                    timeline.MessageFile!.MimeType = message.Image.MimeType;
                    timeline.MessageFile.Caption = message.Image.Caption;
                    string whatsImageId = message.Image.Sha256; //Or message.Image.Id; 
                }

                if (message.Type.Equals("unsupported"))
                {
                    return;
                }

                lead.Timelines.Add(timeline);
            }

            await _leadBrokerBac.ReceiveLeadsAsync(lead);
        }
    }
}
