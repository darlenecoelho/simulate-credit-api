using SimulateCredit.Application.DTOs;

namespace SimulateCredit.Application.Ports.Outgoing;

public interface ICreditSimulationPublisher
{
    /// <summary>
    /// Publishes the result of a simulation to the broker.
    /// </summary>
    Task PublishSimulationAsync(SimulationResult message);
}