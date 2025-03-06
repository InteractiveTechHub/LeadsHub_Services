
using AdaptiveKitCore.Requests;
using WhatsApp.Core.Interfaces.IBac;
using WhatsApp.Core.Interfaces.IRepository;
using WhatsApp.Core.Models;
using WhatsApp.Core.Response;

namespace WhatsApp.Core.Bac
{
    public class WhatsappConfigBac : IWhatsAppConfigBac
    {
        private readonly IWhatsAppConfigRepository _WhatsAppConfigrepository;
        public WhatsappConfigBac(IWhatsAppConfigRepository whatsAppConfigRepository)
        {
            _WhatsAppConfigrepository = whatsAppConfigRepository;
        }

        public async Task<ConfigResponse> FetchConfigByCompanyIdAsync(long companyId)
        {
            ConfigResponse response = await _WhatsAppConfigrepository.FetchConfigByCompanyIdAsync(companyId);
 
            return response;
        }

        public async Task<BaseResponse<Integration>> FetchWhatsappConfigByRequestAsync(FilterRequest filterRequest)
        {
            BaseResponse<Integration> response = await _WhatsAppConfigrepository.FetchWhatsappConfigByRequestAsync(filterRequest);

            return response;
        }
    }
}
