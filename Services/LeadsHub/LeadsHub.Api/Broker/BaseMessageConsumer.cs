using RabbitMQ.Client;

namespace LeadsHub.Api.Broker
{
    public class BaseMessageConsumer : BackgroundService
    {
        protected IConnection _connection = default!;
        protected IChannel _channel = default!;
        private readonly string _queueName;

        protected BaseMessageConsumer(string queueName)
        {
            _queueName = queueName;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            //var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync();

            await _channel.QueueDeclareAsync(_queueName, false, false, false, null, cancellationToken: cancellationToken);

            await base.StartAsync(cancellationToken);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.CompletedTask;
        }

        public override async void Dispose()
        {
            await _channel!.CloseAsync();
            await _connection!.CloseAsync();

            base.Dispose();
        }
    }
}
