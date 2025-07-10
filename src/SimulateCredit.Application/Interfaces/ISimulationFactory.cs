using SimulateCredit.Application.DTOs;
using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Application.Interfaces;

/// <summary>
/// Create the simulation domain entity
/// from the request and the calculated result
/// </summary>
public interface ISimulationFactory
{
    Simulation Create(
        SimulateCreditRequest request,
        SimulationResult result);
}
