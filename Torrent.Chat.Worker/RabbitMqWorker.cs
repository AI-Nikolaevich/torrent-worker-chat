using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Torrent.Chat.Worker.Repositories;

namespace Torrent.Chat.Worker
{
    internal class RabbitMqWorker : BackgroundService
    {
        private readonly ILogger<RabbitMqWorker> _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly DbRepository _dbRepository;
        public RabbitMqWorker(ILogger<RabbitMqWorker> logger, DbRepository dbRepository)
        {
            _logger = logger;
            var factory = new ConnectionFactory { HostName = "93.183.70.206" }; // Default port is 5672
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "chat_messages", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _dbRepository = dbRepository;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() =>
            {
                _logger.LogInformation("Cancellation requested. Closing RabbitMQ connection...");
                _channel.Close();
                _connection.Close();
            });

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (sender, eventArgs) => await Consume(sender, eventArgs);

            _channel.BasicConsume("chat_messages", false, consumer);
            return Task.CompletedTask;
        }

        private async Task Consume(object? sender, BasicDeliverEventArgs eventArgs)
        {
            var content = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            var message = JsonSerializer.Deserialize<DbEntities.ChatEntity>(content);

            _logger.LogInformation("Message: " + message);

            if (message is not null)  await _dbRepository.InsertMessagesAsync(message);

            // Подтверждение получение сообщения
            _channel.BasicAck(eventArgs.DeliveryTag, false);

        }

    }
}
