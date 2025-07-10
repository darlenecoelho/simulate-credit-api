using SimulateCredit.Domain.Enums;
using SimulateCredit.Domain.Interfaces;
using SimulateCredit.Domain.Strategies;

namespace SimulateCredit.Application.Factories;

public static class InterestStrategyFactory
{
    public static IInterestStrategy Create(InterestRateType policyType)
    {
        return policyType switch
        {
            InterestRateType.Age => new FixedInterestStrategy(),
            InterestRateType.AgeWithVariableRate => new VariableInterestStrategy(new FixedInterestStrategy(), 0.015m),
            _ => throw new ArgumentOutOfRangeException(nameof(policyType), "Unknown interest rate type.")
        };
    }
}