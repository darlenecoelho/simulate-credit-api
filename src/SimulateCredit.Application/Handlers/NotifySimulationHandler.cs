using MediatR;
using SimulateCredit.Application.Notifications;
using SimulateCredit.Application.Ports.Outgoing;

namespace SimulateCredit.Application.Handlers;

public class NotifySimulationHandler
    : INotificationHandler<SimulationCompletedNotification>
{
    private readonly INotificationService _notifier;

    public NotifySimulationHandler(INotificationService notifier)
        => _notifier = notifier;

    public Task Handle(SimulationCompletedNotification notify, CancellationToken cancellationToken)
    {
        var r = notify.Result;
        return _notifier.NotifySimulationAsync(
            r.Email,
            r.TotalAmount,
            r.MonthlyPayment,
            r.TotalInterest);
    }
}
