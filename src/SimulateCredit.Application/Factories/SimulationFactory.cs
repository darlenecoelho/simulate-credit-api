using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Application.Factories;

public static class SimulationFactory
{
    public static Simulation Create(
        string email,
        DateTime birthDate,
        decimal loanAmount,
        string loanCurrency,
        int months,
        string rateType,
        string sourceCurrency,
        string targetCurrency,
        decimal totalAmount,
        decimal monthlyPayment,
        decimal totalInterest)
    {
        return new Simulation(
            email,
            birthDate,
            loanAmount,
            loanCurrency,
            months,
            rateType,
            sourceCurrency,
            targetCurrency,
            totalAmount,
            monthlyPayment,
            totalInterest
        );
    }
}
