
using WhatsApp.Core.Request;
using WhatsApp.Core.Utility;
using WhatsApp.Core.Interfaces.IBac;
using WhatsApp.Core.Interfaces.IServices;
using WhatsApp.Core.Response;

namespace WhatsApp.Core.Services
{
    public sealed class TemplateService : ITemplateService
    {
        private readonly IBaseService _baseService;
        private readonly IWhatsAppConfigBac _whatsAppConfigBac;

        public TemplateService(IBaseService baseService,
            IWhatsAppConfigBac whatsAppConfigBac)
        {
            _baseService = baseService;
            _whatsAppConfigBac = whatsAppConfigBac;
        }

        public async Task<JsonResponse> FetchTemplatesAsync(long empresaId)
        {
            MessageRequest request = new();

            ConfigResponse response = await _whatsAppConfigBac.FetchConfigByCompanyIdAsync(empresaId);
            if (response.HasExceptionMessage)
            {
                //TODO: should not continue
                return new();
            }

            request.AccessToken = response.ResponseData.Select(r => r.AccessToken).FirstOrDefault() ?? string.Empty;
            string businessAccount = response.ResponseData.Select(r => r.BusinessAccountId).FirstOrDefault() ?? string.Empty;

            request.Url = SD.WhatsappAPIBase + $"/{businessAccount}/message_templates";

            JsonResponse jsonResponse = await _baseService.GetAsync(request);

            return jsonResponse;
        }
    }
}
