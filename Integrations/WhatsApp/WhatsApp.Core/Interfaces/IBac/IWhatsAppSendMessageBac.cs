using WhatsApp.Core.PayLoads;

namespace WhatsApp.Core.Interfaces.IBac
{
    public interface IWhatsappSendMessageBac
    {
        Task ReceiveMessageFromWhatsappAsync(WhatsappPayLoad whatsappMessage);
    }
}
