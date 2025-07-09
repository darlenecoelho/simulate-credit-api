using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Domain.Interfaces;

public interface IInterestStrategy
{
    decimal GetAnnualRate(Customer customer);
}