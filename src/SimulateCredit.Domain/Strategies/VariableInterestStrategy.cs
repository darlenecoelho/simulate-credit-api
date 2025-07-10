using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Interfaces;

namespace SimulateCredit.Domain.Strategies;

public sealed class VariableInterestStrategy : IInterestStrategy
{
    private readonly IInterestStrategy _baseStrategy;
    private readonly decimal _extraAnnualRate;

    public VariableInterestStrategy(IInterestStrategy baseStrategy, decimal extraAnnualRate)
    {
        _baseStrategy = baseStrategy;
        _extraAnnualRate = extraAnnualRate;
    }

    public decimal GetAnnualRate(Customer customer)
    {
        if (customer is null)
            throw new ArgumentNullException(nameof(customer));

        return _baseStrategy.GetAnnualRate(customer) + _extraAnnualRate;
    }
}
