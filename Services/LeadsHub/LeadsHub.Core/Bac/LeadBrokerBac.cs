
using AdaptiveKitCore.Model;
using AdaptiveKitCore.Responses;
using AdaptiveKitCore.Responses.Interfaces;
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

        public async Task ReceiveLeadsAsync(Lead lead)
        {
            SimpleResponse<Lead?> response = await _leadBrokerRepository.FetchLeadByContactAsync(lead.Contact);

            // if contact is not found, register a new lead
            if (response.Model is null)
            {
                await CreatesNewLeadAsync(lead);

                return;
            }

            response.Model.Timelines = [..lead.Timelines];
            lead = response.Model;

            await RegisterLeadMessageAsync(lead);
        }

        private async Task<SimpleResponse<Lead>> CreatesNewLeadAsync(Lead lead)
        {
            long contactId = await VerifyContactAsync(lead.Contact);

            Consultant? consultant = await _distributionService.DistributeLeadsAsync(lead.CompanyId);

            lead.Contact.Id = contactId;
            lead.ContactId = contactId;
            lead.ConsultantId = consultant.Id;
            lead.Consultant = consultant;

            SimpleResponse<Lead> leadResponse = await _leadBrokerRepository.RegisterLeadAsync(lead);
            if (leadResponse.HasAnyErrorMessage)
            {
                // TODO: must implement a log here
                return leadResponse;
            }

            lead.Id = leadResponse.Model!.Id;
            lead.Identifier = leadResponse.Model.Identifier;

            if (consultant is not null)
            {                
                consultant.TimeLastLeadAssigned = DateTimeOffset.UtcNow;
                await _leadBrokerRepository.UpdateConsultantsAsync(consultant);
            }            

            await RegisterLeadMessageAsync(lead);

            await NotifyNewLeadToManagerAsync(lead);

            return leadResponse;
        }

        private async Task NotifyNewMessage(Lead lead)
        {
            await _hubContext.Clients.Group($"lead-{lead.Identifier.ToString()}").SendAsync("newMessage");
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
        private async Task RegisterLeadMessageAsync(Lead lead)
        {
            foreach (Timeline timeline in lead.Timelines)
            {
                timeline.LeadId = lead.Id;
                timeline.UpdatedAt = DateTimeOffset.UtcNow;

                if (timeline.Type.Equals(1))
                {
                    var response = await _timelineRepository.RegisterMessageTextAsync(timeline);
                }

                if (timeline.Type.Equals(2))
                {
                    // Update or register reaction
                    //TODO: Try to find reaction (insert or update), Fetch the message to get the message body
                    // Update frontend timeline with the reaction.
                }

                if (timeline.Type.Equals(3))
                {
                    // TODO: Mark as deleted when the message is deleted;

                    // Register image
                    var response = await _timelineRepository.RegisterMessageFileAsync(timeline);
                }                
            }

            await NotifyNewMessage(lead);
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
