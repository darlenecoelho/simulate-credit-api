namespace SimulateCredit.Infrastructure.Settings;

public sealed class RabbitMqSettings
{
    public required string HostName { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
    public required string QueueName { get; init; }
}