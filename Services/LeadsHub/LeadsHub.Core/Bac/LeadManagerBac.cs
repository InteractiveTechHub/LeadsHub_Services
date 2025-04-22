
using AdaptiveKitCore.Enums;
using AdaptiveKitCore.Requests;
using AdaptiveKitCore.Responses;
using LeadsHub.Core.Dtos;
using LeadsHub.Core.Enum;
using LeadsHub.Core.Extensions;
using LeadsHub.Core.Interfaces.IBac;
using LeadsHub.Core.Interfaces.IRepository;
using LeadsHub.Core.Models;
using LeadsHub.Core.Responses;
using PhoneNumbers;

namespace LeadsHub.Core.Bac
{
    public sealed class LeadManagerBac : ILeadManagerBac
    {
        private readonly ILeadManagerRepository _managerRepository;
        private readonly ILeadRepository _leadRepository;
        private readonly IWhatsAppRepository _whatsAppRepository;

        public LeadManagerBac(ILeadManagerRepository leadManagerRepository, ILeadRepository leadRepository, IWhatsAppRepository whatsAppRepository)
        {
            _managerRepository = leadManagerRepository;
            _leadRepository = leadRepository;
            _whatsAppRepository = whatsAppRepository;
        }

        public async Task<LeadCardResponse> FetchCardsByRequestAsync(FilterRequest filterRequest)
        {
            var response = await _managerRepository.FetchCardsByRequestAsync(filterRequest);
            if (response.HasAnyErrorMessage)
            {
                return response;
            }

            response.ResponseData.ForEach((card) =>
            {
                card.PhoneNumber = card.PhoneNumber.FormatPhoneNumber();
            });

            return response;
        }

        public async Task<BaseResponse<TemplatesPerType>> FetchTemplatesAsync(long leadId)
        {
            BaseResponse<TemplatesPerType> response = new();

            SimpleResponse<Lead?> leadResponse = await _leadRepository.FetchLeadByIdAsync(leadId);
            if (leadResponse.HasAnyErrorMessage)
            {
                response.Messages = leadResponse.Messages;
                return response;
            }

            if (leadResponse.Model is null)
            {
                return new();
            }   

            Lead lead = leadResponse.Model;

            FilterRequest filterRequest = new();
            filterRequest.AddFilter(nameof(Integration.Id), FilterOperatorEnum.Equals, lead.IntegrationId, "i");
            filterRequest.AddFilter(nameof(WhatsAppTemplate.Status), FilterOperatorEnum.Equals, "APPROVED", "wt");
            filterRequest.AddFilter(nameof(WhatsAppTemplate.Enabled), FilterOperatorEnum.Equals, true, "wt");

            if (lead.Phase == LeadPhase.New)
            {
                // fetch WelcomeMessage templates
                filterRequest.AddFilter(nameof(WhatsAppTemplate.Type), FilterOperatorEnum.Equals, TemplateType.WelcomeMessage, "wt");
            }

            if (lead.Phase == LeadPhase.InProgress)
            {
                // fetch Followup templates
                filterRequest.AddFilter(nameof(WhatsAppTemplate.Type), FilterOperatorEnum.Equals, TemplateType.FollowUp, "wt");
            }

            if (lead.Phase == LeadPhase.Appointment)
            {
                // fetch Followup templates and Appointment templates
                filterRequest.AddFilter(nameof(WhatsAppTemplate.Type), FilterOperatorEnum.Equals, FilterConnectorEnum.OR, TemplateType.FollowUp, "wt");
                filterRequest.AddFilter(nameof(WhatsAppTemplate.Type), FilterOperatorEnum.Equals, TemplateType.Appointment, "wt");
            }

            if (lead.Phase == LeadPhase.Closed)
            {
                // fetch Feed back templates;
                filterRequest.AddFilter(nameof(WhatsAppTemplate.Type), FilterOperatorEnum.Equals, TemplateType.CustomerFeedback, "wt");
            }

            var integrationResponse = await _whatsAppRepository.FetchWhatsappConfigByRequestAsync(filterRequest);
            if (integrationResponse.HasAnyErrorMessage)
            {
                response.Messages = integrationResponse.Messages;
                return response;
            }

            Integration? integration = integrationResponse.ResponseData.FirstOrDefault();
            List<WhatsAppTemplate>? templates = integration?.WhatsAppConfig?.WhatsAppTemplates;

            if (templates is null || !templates.Any())
            {
                return new();
            }

            List<TemplatesPerType> groupedTemplates = templates.GroupBy(t => t.Type).Select(g => new TemplatesPerType 
            {
                TemplateType = g.Key,
                Templates = g.Select(t => new WhatsAppTemplateDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    TemplateBodyMirror = t.TemplateBodyMirror,
                    TemplateType = t.Type
                }).ToList()
            }).ToList();

            response.ResponseData = groupedTemplates;

            return response;
        }

        public async Task<SimpleResponse<Lead?>> CloseLeadAsync(LeadCard leadCard)
        {
            SimpleResponse<Lead?> response = await _leadRepository.FetchLeadByIdAsync(leadCard.LeadId);
            if (response.HasAnyErrorMessage || response.Model is null)
            {
                return response;
            }

            response.Model!.Status = leadCard.Status;
            response.Model!.Phase = LeadPhase.Closed;
            response.Model!.SaleNote = leadCard.SaleNote;

            response = await _leadRepository.UpdateLeadAsync(response.Model);

            return response;
        }
    }
}
