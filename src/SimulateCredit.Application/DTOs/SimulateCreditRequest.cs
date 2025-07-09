using SimulateCredit.Domain.Enums;
using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.DTOs;

public sealed class SimulateCreditRequest
{
    public LoanAmountDto LoanAmount { get; init; } = new();
    public CustomerDetailsDto Customer { get; init; } = new();
    public int Months { get; init; }

    public InterestRateType RateType { get; init; } = InterestRateType.Age;
    public Currency SourceCurrency { get; init; } = Currency.From("BRL");
    public Currency TargetCurrency { get; init; } = Currency.From("BRL");
}
