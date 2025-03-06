using WhatsApp.Core.Response;

namespace WhatsApp.Core.Interfaces.IServices
{
    public interface ITemplateService
    {
        Task<JsonResponse> FetchTemplatesAsync(long empresaId);
    }
}
