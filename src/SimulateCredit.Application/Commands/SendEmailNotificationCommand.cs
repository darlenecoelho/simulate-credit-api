using MediatR;
using SimulateCredit.Application.DTOs;

namespace SimulateCredit.Application.Commands;

public sealed record SendEmailNotificationCommand(SimulationResult Message)
    : IRequest;