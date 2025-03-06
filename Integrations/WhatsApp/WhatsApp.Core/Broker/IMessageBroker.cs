
namespace WhatsApp.Core.Broker
{
    public interface IMessageBroker
    {
        Task SendMessageAsync(object message, string queueName);
    }
}
