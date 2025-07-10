using SimulateCredit.Application.DTOs;

namespace SimulateCredit.Application.Interfaces;

/// <summary>
/// Responsible only for executing the PMT formula.
/// (currency conversion + interest and installment calculation).
/// </summary>
public interface ILoanCalculationService
{
    Task<SimulationResult> CalculateAsync(SimulateCreditRequest request);
}
