using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Infrastructure.Settings;
using System.Text.Json;

namespace SimulateCredit.Infrastructure.Adapters.Messaging;

public sealed class RabbitMqPublisherAdapter : ICreditSimulationPublisher, IDisposable
{
    private readonly IConnection _connection;
    private readonly RabbitMqSettings _settings;

    public RabbitMqPublisherAdapter(IOptions<RabbitMqSettings> options)
    {
        _settings = options.Value;
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

            channel.BasicPublish(
                exchange: "",
                routingKey: _settings.QueueName,
                basicProperties: props,
                body: body);
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
