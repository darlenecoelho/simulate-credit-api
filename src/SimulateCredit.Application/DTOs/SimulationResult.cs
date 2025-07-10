namespace SimulateCredit.Application.DTOs;

public sealed record SimulationResult
{
    public decimal TotalAmount { get; init; }
    public decimal MonthlyPayment { get; init; }
    public decimal TotalInterest { get; init; }
    public required string Email { get; init; }
}
