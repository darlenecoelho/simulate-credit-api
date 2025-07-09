using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Factories;
using SimulateCredit.Application.Ports.Incoming;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.UseCases.SimulateCredit;

public sealed class SimulateCreditUseCase : ISimulateCreditUseCase
{
    private readonly ICurrencyConverterService _currencyConverter;
    private readonly ISimulationRepository _simulationRepository;

    public SimulateCreditUseCase(ICurrencyConverterService currencyConverter, ISimulationRepository simulationRepository)
    {
        _currencyConverter = currencyConverter;
        _simulationRepository = simulationRepository;
    }

    public async Task<SimulateCreditResponse> ExecuteAsync(SimulateCreditRequest request)
    {
        var customer = new Customer(request.Customer.BirthDate, request.Customer.Email);
        var strategy = InterestStrategyFactory.Create(request.RateType);
        var annualRate = strategy.GetAnnualRate(customer);
        var monthlyRate = annualRate / 12m;

        var convertedAmount = await _currencyConverter.ConvertAsync(
       request.LoanAmount.Amount,
       request.SourceCurrency,
       request.TargetCurrency
       );

        var money = new Money(convertedAmount);
        var n = request.Months;
        var r = monthlyRate;
        var p = money.Amount;

        var factor = (decimal)Math.Pow(1 + (double)r, n); 
        var monthlyPayment = (p * r * factor) / (factor - 1);
        var total = monthlyPayment * n;
        var totalInterest = total - p;

        var roundedMonthlyPayment = Math.Round(monthlyPayment, 2);
        var roundedTotal = Math.Round(total, 2);
        var roundedInterest = Math.Round(totalInterest, 2);

        var simulation = SimulationFactory.Create(
         request.Customer.Email,
         request.Customer.BirthDate,
         p,
         request.LoanAmount.Currency,
         request.Months,
         request.RateType.ToString(),
         request.SourceCurrency,
         request.TargetCurrency,
         roundedTotal,
         roundedMonthlyPayment,
         roundedInterest
     );

        await _simulationRepository.SaveAsync(simulation);

        return new SimulateCreditResponse
        {
            TotalAmount = roundedTotal,
            MonthlyPayment = roundedMonthlyPayment,
            TotalInterest = roundedInterest
        };
    }
}