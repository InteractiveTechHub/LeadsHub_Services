
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using CrossCutting.Models;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Interfaces.IServices;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;

namespace LeadsHub.Core.Bac
{
    public sealed class TimelineBac : ITimelineBac
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ISendMessageService _sendMessageService;
        private readonly ITimelineRepository _timelineRepository;
        public TimelineBac(ILeadRepository leadRepository, ISendMessageService sendMessageService, ITimelineRepository timelineRepository)
        {
            _leadRepository = leadRepository;
            _sendMessageService = sendMessageService;
            _timelineRepository = timelineRepository;
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

            // text
            if (timeline.Type == 1)
            {
                response = await _timelineRepository.RegisterMessageTextAsync(timeline);

                await SendTextMessageAsync(timeline);
            }

            // template
            if (timeline.Type == 2)
            {

            }

            // file
            if (timeline.Type == 3)
            {

            }

            // reaction
            if (timeline.Type == 4)
            {

            }

            return response;
        }

        /// <summary>
        /// The only responsability of this method is send message of kind text
        /// </summary>
        /// <param name="timeline">Message to send</param>
        /// <returns></returns>
        private async Task SendTextMessageAsync(Timeline timeline)
        {
            SimpleResponse<Lead?> response = await _leadRepository.FetchLeadByIdAsync(timeline.LeadId);
            if (response.HasAnyErrorMessage || response.Model is null)
            {
                return;
            }

            Lead lead = response.Model;

            TransferLead transfer = new();
            transfer.IntegrationId = lead.IntegrationId;
            transfer.CompanyId = lead.CompanyId;
            transfer.PhoneNumber = lead.Contact.PhoneNumber;
            transfer.MessageType = timeline.Type;
            transfer.MessageDate = timeline.MessageDate;
            transfer.MessageBody = timeline.Message!.Body;

            var sendResponse = await _sendMessageService.SendMessageToWhatsApp(transfer);
        }
    }
}
