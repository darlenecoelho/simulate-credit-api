using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Interfaces;
using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Application.Factories;

public class SimulationFactoryAdapter : ISimulationFactory
{
    public Simulation Create(
        SimulateCreditRequest req,
        SimulationResult result)
    {
        return SimulationFactory.Create(
            email: req.Customer.Email,
            birthDate: req.Customer.BirthDate,
            loanAmount: req.LoanAmount.Amount,
            loanCurrency: req.LoanAmount.Currency.ToString(),  
            months: req.Months,
            rateType: req.RateType.ToString(),
            sourceCurrency: req.SourceCurrency.ToString(),         
            targetCurrency: req.TargetCurrency.ToString(),
            totalAmount: result.TotalAmount,
            monthlyPayment: result.MonthlyPayment,
            totalInterest: result.TotalInterest
        );
    }
}