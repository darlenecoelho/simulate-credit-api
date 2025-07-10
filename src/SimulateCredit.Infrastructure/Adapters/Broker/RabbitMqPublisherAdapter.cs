using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Infrastructure.Settings;
using System.Text.Json;

namespace SimulateCredit.Infrastructure.Adapters.Broker;

public sealed class RabbitMqPublisherAdapter : ICreditSimulationPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly RabbitMqSettings _settings;
    private readonly ILogger<RabbitMqPublisherAdapter> _logger;

    public RabbitMqPublisherAdapter(IOptions<RabbitMqSettings> options, ILogger<RabbitMqPublisherAdapter> logger)
    {
        _settings = options.Value;
        _logger = logger;
        var factory = new ConnectionFactory
        {
            HostName = _settings.HostName,
            UserName = _settings.UserName,
            Password = _settings.Password,
            DispatchConsumersAsync = true
        };
        _connection = factory.CreateConnection();
    }

    public Task PublishSimulationAsync(SimulationResult message)
    {
        return Task.Run(() =>
        {
            using var channel = _connection.CreateModel();
            channel.QueueDeclare(
                queue: _settings.QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = JsonSerializer.SerializeToUtf8Bytes(message);
            var props = channel.CreateBasicProperties();
            props.Persistent = true;

            try
            {
                channel.BasicPublish(
                    exchange: "",
                    routingKey: _settings.QueueName,
                    basicProperties: props,
                    body: body);
                _logger.LogInformation("Simulation published to RabbitMQ queue {Queue}", _settings.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing simulation to RabbitMQ");
                throw;
            }
        });
    }

    public void Dispose()
    {
        if (_connection.IsOpen)
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}