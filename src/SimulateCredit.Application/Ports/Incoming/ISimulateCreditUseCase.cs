using SimulateCredit.Application.DTOs;

namespace SimulateCredit.Application.Ports.Incoming;

public interface ISimulateCreditUseCase
{
    Task<SimulateCreditResponse> ExecuteAsync(SimulateCreditRequest request);
}