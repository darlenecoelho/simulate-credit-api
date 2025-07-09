namespace SimulateCredit.Application.DTOs;

public sealed class SimulateCreditResponse
{
    public decimal TotalAmount { get; init; }
    public decimal MonthlyPayment { get; init; }
    public decimal TotalInterest { get; init; }
}