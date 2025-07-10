using MediatR;
using SimulateCredit.Application.Notifications;
using SimulateCredit.Application.Ports.Outgoing;

namespace SimulateCredit.Application.Handlers;

public class PublishSimulationHandler
    : INotificationHandler<SimulationCompletedNotification>
{
    private readonly ICreditSimulationPublisher _publisher;

    public PublishSimulationHandler(ICreditSimulationPublisher publisher)
        => _publisher = publisher;

    public Task Handle(SimulationCompletedNotification notify, CancellationToken cancellationToken)
    {
        return _publisher.PublishSimulationAsync(notify.Result);
    }
}
