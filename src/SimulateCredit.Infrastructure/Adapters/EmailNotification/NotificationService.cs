using SimulateCredit.Application.Ports.Outgoing;

namespace SimulateCredit.Infrastructure.Adapters.EmailNotification;

public class NotificationService : INotificationService
{
    private readonly IAuditLogger _auditLogger;

    public NotificationService(IAuditLogger auditLogger)
        => _auditLogger = auditLogger;

    public Task NotifySimulationAsync(
        string email,
        decimal totalAmount,
        decimal monthlyPayment,
        decimal totalInterest)
    {
        try
        {
            _auditLogger.LogInformation(
                "Simulating send email to {Email}: Total={Total:C}, Monthly={Monthly:C}, Interest={Interest:C}",
                email, totalAmount, monthlyPayment, totalInterest);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _auditLogger.LogError(ex, "Error sending notification to {Email}", email);
            throw;
        }
    }
}