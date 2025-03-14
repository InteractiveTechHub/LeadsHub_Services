
using LeadsHub.Core.Interfaces.IBac;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace LeadsHub.Api.Broker
{
   /* public class MessageConsumer : BaseMessageConsumer
    {
        private readonly ILeadBrokerBac _leadBrokerBac;

        public MessageConsumer(ILeadBrokerBac leadBrokerBac) : base("leadmessage")
        {
            _leadBrokerBac = leadBrokerBac;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += async (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        string? mensagem = Encoding.UTF8.GetString(body);

                        TransferLead message = JsonSerializer.Deserialize<TransferLead>(mensagem)!;

                        await _leadBrokerBac.ReceiveLeadsAsync(message);

                        _channel?.BasicAckAsync(ea.DeliveryTag, false);
                    }
                    catch (Exception ex) 
                    {
                        _channel?.BasicAckAsync(ea.DeliveryTag, false);
                    }
                };

                await _channel.BasicConsumeAsync(queue: "leadmessage", autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error on MessageConsumer.ExecuteAsync: {ex.Message}");
            }
        }
    }*/
}
