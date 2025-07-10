using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Factories;
using SimulateCredit.Application.Interfaces;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Domain.Entities;

namespace SimulateCredit.Application.Services;

public class LoanCalculationService : ILoanCalculationService
{
    private readonly ICurrencyConverterService _converter;

    public LoanCalculationService(ICurrencyConverterService converter)
    {
        _converter = converter;
    }

    public async Task<SimulationResult> CalculateAsync(SimulateCreditRequest request)
    {
        // 1) converte valor
        var converted = await _converter.ConvertAsync(
            request.LoanAmount.Amount,
            request.SourceCurrency,
            request.TargetCurrency);

        // 2) determina juros pelo tipo de taxa
        var customer = new Customer(request.Customer.BirthDate, request.Customer.Email);
        var strategy = InterestStrategyFactory.Create(request.RateType);
        var annualRate = strategy.GetAnnualRate(customer);
        var monthlyRate = annualRate / 12m;

        // 3) aplica fórmula de parcelas fixas (PMT)
        var n = request.Months;
        var r = monthlyRate;
        var p = converted;
        var factor = (decimal)Math.Pow(1 + (double)r, n);
        var pay = (p * r * factor) / (factor - 1);
        var total = pay * n;
        var interest = total - p;

        // 4) monta DTO já arredondado
        return new SimulationResult
        {
            TotalAmount = Math.Round(total, 2),
            MonthlyPayment = Math.Round(pay, 2),
            TotalInterest = Math.Round(interest, 2),
            Email = request.Customer.Email
        };
    }
}
