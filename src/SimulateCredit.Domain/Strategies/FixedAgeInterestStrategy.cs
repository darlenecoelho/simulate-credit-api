using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Enums;
using SimulateCredit.Domain.Interfaces;

namespace SimulateCredit.Domain.Strategies;

public sealed class FixedInterestStrategy : IInterestStrategy
{
    public decimal GetAnnualRate(Customer customer)
    {
        if (customer == null)
            throw new ArgumentNullException(nameof(customer));

        return customer.GetInterestRateGroup() switch
        {
            InterestAgeGroup.UP_TO_25 => 0.05m,
            InterestAgeGroup.FROM_26_TO_40 => 0.03m,
            InterestAgeGroup.FROM_41_TO_60 => 0.02m,
            InterestAgeGroup.ABOVE_60 => 0.04m,
            _ => throw new InvalidOperationException("Unknown age group.")
        };
    }
}
