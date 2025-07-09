using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Application.Ports.Outgoing;

public interface ISimulationRepository
{
    Task SaveAsync(Simulation simulation);
}