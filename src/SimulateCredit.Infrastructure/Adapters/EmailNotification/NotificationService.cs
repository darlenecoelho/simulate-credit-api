using Microsoft.Extensions.Logging;
using SimulateCredit.Application.Ports.Outgoing;

namespace SimulateCredit.Infrastructure.Adapters.EmailNotification;

public class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
        => _logger = logger;

    public Task NotifySimulationAsync(
        string email,
        decimal totalAmount,
        decimal monthlyPayment,
        decimal totalInterest)
    {
        _logger.LogInformation(
            "Simulating send email to {Email}: Total={Total:C}, Monthly={Monthly:C}, Interest={Interest:C}",
            email, totalAmount, monthlyPayment, totalInterest);

        return Task.CompletedTask;
    }
}
