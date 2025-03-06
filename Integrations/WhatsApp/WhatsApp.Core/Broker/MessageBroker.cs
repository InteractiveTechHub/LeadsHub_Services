
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace WhatsApp.Core.Broker
{
    public class MessageBroker : IMessageBroker
    {
        private readonly string _hostName;
        private readonly string _userName;
        private readonly string _password;

        private IConnection? _connection;

        public MessageBroker()
        {
            _hostName = "rabbitmq";
            _password = "guest";
            _userName = "guest";
        }

        public async Task SendMessageAsync(object message, string queueName)
        {
            if (await ConnectiionExistsAsync())
            {
                using var channel = await _connection!.CreateChannelAsync();
                await channel.QueueDeclareAsync(queueName, false, false, false, null);

                string? json = JsonSerializer.Serialize(message);
                byte[] body = Encoding.UTF8.GetBytes(json);

                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
            }
        }

        private async Task CreateConnectionAsync()
        {
            try
            {
                ConnectionFactory connectionFactory = new()
                {
                    HostName = _hostName,
                    UserName = _userName,
                    Password = _password
                };

                _connection = await connectionFactory.CreateConnectionAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> ConnectiionExistsAsync()
        {
            if (_connection != null)
            {
                return true;
            }

            await CreateConnectionAsync();

            return true;
        }
    }
}
