using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.DTOs;

public sealed class LoanAmountDto
{
    public decimal Amount { get; init; }
    public Currency Currency { get; init; } = Currency.From("BRL");
}
