
using AdaptiveKitCore.Responses;
using CrossCutting.Models;
using LeadsHub.Core.Hubs;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace LeadsHub.Core.Bac
{
    public sealed class LeadBrokerBac : ILeadBrokerBac
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDistributionService _distributionService;
        private readonly ILeadBrokerRepository _leadBrokerRepository;
        private readonly ITimelineRepository _timelineRepository;      

        private readonly IHubContext<LeadCardHub> _hubContext;

        public LeadBrokerBac(
            ICustomerRepository customerRepository,
            IDistributionService distributionService,
            IHubContext<LeadCardHub> hubContext,
            ILeadBrokerRepository leadBrokerRepository,
            ITimelineRepository timelineRepository)
        {
            _customerRepository = customerRepository;
            _distributionService = distributionService;
            _hubContext = hubContext;
            _leadBrokerRepository = leadBrokerRepository;
            _timelineRepository = timelineRepository;
        }

        public async Task ReceiveLeadsAsync(TransferLead transferLead)
        {
            Contact contact = new()
            {
                Name = transferLead.Name,
                PhoneNumber = transferLead.PhoneNumber,
                Email = transferLead.Email,
                //CPF = leadMessage.CPF,
                //BirthDate = leadMessage.BirthDate,
            };

            SimpleResponse<Lead?> response = await _leadBrokerRepository.FetchLeadByContactAsync(contact);

            // if contact is not found, register a new lead
            if (response.Model is null)
            {
                response = await CreatesNewLeadAsync(transferLead, contact);

                await NotifyNewLeadToManagerAsync(response.Model!);
            }            

            await RegisterLeadMessageAsync(transferLead, response.Model!);

            //await NotifyLeadManagerAsync(response.Model, isNewLead);
        }

        private async Task<SimpleResponse<Lead?>> CreatesNewLeadAsync(TransferLead transferLead, Contact contact)
        {
            long contactId = await VerifyContactAsync(contact);

            Consultant? consultant = await _distributionService.DistributeLeadsAsync(transferLead.CompanyId);

            Lead lead = new()
            {
                CompanyId = transferLead.CompanyId,
                ConsultantId = consultant?.Id,
                ContactId = contactId,
                Status = 1, // (Awaiting answer)
                IntegrationId = transferLead.IntegrationId,
            };

            SimpleResponse<Lead> leadResponse = await _leadBrokerRepository.RegisterLeadAsync(lead);
            if (leadResponse.HasErrorMessage)
            {
                // TODO: must implement a log here
                return leadResponse;
            }

            lead.Id = leadResponse.Model!.Id;

            if (consultant is not null)
            {                
                consultant.TimeLastLeadAssigned = DateTimeOffset.UtcNow;
                await _leadBrokerRepository.UpdateConsultantsAsync(consultant);

                lead.Consultant = consultant;
            }

            return leadResponse;
        }

        private async Task NotifyNewMessage(Lead lead)
        {
            if (lead.Consultant is not null && !string.IsNullOrEmpty(lead.Consultant!.IdentityId))
            {
                // should updage messages without pulling, sending object.
                await _hubContext.Clients.User(lead.Consultant.IdentityId).SendAsync("newMessage");
            }
        }

        private async Task NotifyNewLeadToManagerAsync(Lead lead)
        {
            if (lead.Consultant is not null && !string.IsNullOrEmpty(lead.Consultant!.IdentityId))
            {
                // Should update the lead list avoid pulling, sending object.
                await _hubContext.Clients.User(lead.Consultant.IdentityId).SendAsync("newLead");
            }
        }

        /// <summary>
        /// Register chat message of the lead
        /// </summary>
        /// <param name="message">Lead Message</param>
        /// <param name="lead"></param>
        private async Task RegisterLeadMessageAsync(TransferLead transferLead, Lead lead)
        {
            Timeline timeline = new();

            timeline.LeadId = lead.Id;
            timeline.ConsultantId = lead.Consultant?.Id;
            timeline.Sender = 1;
            timeline.MessageId = transferLead.MessageId;
            timeline.MessageDate = transferLead.MessageDate;
            timeline.Type = 1; // Text
            timeline.Status = 1; // Pending
            timeline.UpdatedAt = DateTimeOffset.UtcNow;

            if (transferLead.MessageType.Equals(1))
            {
                MessageText message = new();
                message.Body = transferLead.MessageBody;

                timeline.Message = message;

                //TODO: Update Message when it is edited;
                //TODO: Delete Message when it is deleted or mark as deleted;

                var response = await _timelineRepository.RegisterMessageTextAsync(timeline);
            }

            if (transferLead.MessageType.Equals("reaction"))
            {
                MessageReaction reaction = new();
                reaction.Emoji = transferLead.ReactionEmoji;
                reaction.MessageId = transferLead.MessageId;

                // Update or register reaction
                //TODO: Try to find reaction (insert or update), Fetch the message to get the message body
                // Update frontend timeline with the reaction.
            }

            if (transferLead.MessageType.Equals("image"))
            {
                MessageFile message = new();
                message.Caption = transferLead.Caption;
                message.Url = transferLead.Url;
                message.MimeType = transferLead.MimeType;

                // TODO: Mark as deleted when the message is deleted;

                // Register image
                var response = await _timelineRepository.RegisterMessageFileAsync(timeline);
            }

            // Send notification via SignalR
        }

        private async Task<long> VerifyContactAsync(Contact contact)
        {
            SimpleResponse<long> response = await _customerRepository.FetchContactIdAsync(contact);
            if (response.HasAnyErrorMessage)
            {
                throw new Exception(response.Messages[0].MessageText);
            }

            if (response.Model == 0)
            {
                response = await _customerRepository.RegisterContactAsync(contact);
                if (response.HasAnyErrorMessage)
                {
                    throw new Exception(response.Messages[0].MessageText);
                }
            }

            long customerId = response.Model;

            return customerId;
        }
    }
}
