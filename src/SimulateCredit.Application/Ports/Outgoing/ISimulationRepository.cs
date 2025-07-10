using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Application.Ports.Outgoing;

public interface ISimulationRepository
{
    /// <summary>
    /// Persists a simulation entity to the database by mapping it to a document format.
    /// </summary>
    /// <param name="simulation">The domain entity containing simulation data to be saved.</param>
    /// <returns>A task representing the async database insertion operation.</returns>
    Task SaveAsync(Simulation simulation);
}