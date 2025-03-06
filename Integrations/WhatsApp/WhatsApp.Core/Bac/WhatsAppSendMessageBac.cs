using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using CrossCutting.Models;
using WhatsApp.Core.Broker;
using WhatsApp.Core.Interfaces.IBac;
using WhatsApp.Core.Models;
using WhatsApp.Core.PayLoads;

namespace whatsapp.Core.Bac
{
    public sealed class WhatsAppSendMessageBac : IWhatsappSendMessageBac
    {
        private readonly IMessageBroker _messageBroker;
        private readonly IWhatsAppConfigBac _configBac;

        public WhatsAppSendMessageBac(IMessageBroker messageBroker, IWhatsAppConfigBac configBac)
        {
            _messageBroker = messageBroker;
            _configBac = configBac;
        }

        public async Task ReceiveMessageFromWhatsappAsync(WhatsappPayLoad whatsappMessage)
        {
            List<TransferLead> transferLeads = new();

            foreach (var entry in whatsappMessage.Entry)
            {
                foreach (PayLoadChange change in entry.Changes)
                {
                    TransferLead transferLead = new();

                    string phoneNumberId = change.Value.Metadata.PhoneNumberId;

                    FilterRequest filter = new();
                    filter.AddFilter(nameof(WhatsAppConfig.PhoneNumberId), FilterOperatorEnum.Equals, phoneNumberId, "w");

                    var response = await _configBac.FetchWhatsappConfigByRequestAsync(filter);
                    if (response.HasAnyErrorMessage)
                    {
                        return;
                    }

                    transferLead.CompanyId = response.ResponseData.Select(r => r.CompanyId).FirstOrDefault();                  
                    transferLead.Name = change.Value.Contacts.Select(c => c.Profile.Name).FirstOrDefault() ?? string.Empty;

                    foreach(PayLoadMessage message in change.Value.Messages)
                    {
                        transferLead.IntegrationId = response.ResponseData.Select(r => r.Id).FirstOrDefault();
                        transferLead.PhoneNumber = message.From;       
                        transferLead.MessageId = message.Id;
                        transferLead.Channel = "Whatsapp";

                        transferLead.ConvertsTimeUnixToUtcDateTime(message.TimeStamp);                   

                        if (message.Type.Equals("text"))
                        {
                            transferLead.MessageType = 1;
                            transferLead.MessageBody = message.Text.Body;                                            
                        }

                        //TODO: Implement others feature later.
                        if (message.Type.Equals("reaction"))
                        {
                            transferLead.ReactionEmoji = message.Reaction.Emoji;
                            transferLead.MessageReactionId = message.Reaction.MessageId;
                        }

                        if (message.Type.Equals("image"))
                        {
                            transferLead.MimeType = message.Image.MimeType;
                            transferLead.Caption = message.Image.Caption;
                            string whatsImageId = message.Image.Id; //Or Sha256???
                        }

                        if (message.Type.Equals("unsupported"))
                        {
                            return;
                        }

                        transferLeads.Add(transferLead);
                    }
                }
            };

            transferLeads.ForEach(async message => await _messageBroker.SendMessageAsync(message, "leadmessage"));
        }
    }
}
