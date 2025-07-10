using MediatR;
using SimulateCredit.Application.DTOs;

namespace SimulateCredit.Application.Notifications;

public sealed record SimulationCompletedNotification(
    SimulationResult Result
) : INotification;
