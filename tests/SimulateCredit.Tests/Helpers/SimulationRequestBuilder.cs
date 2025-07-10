using SimulateCredit.Application.DTOs;
using SimulateCredit.Domain.Enums;
using SimulateCredit.Domain.ValueObjects;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Helpers;

public class SimulationRequestBuilder : TestConfig<Currency>
{
    private readonly DateTime _birth;

    public SimulationRequestBuilder(bool Above60)
    {
        _birth = Above60 ? DateTime.UtcNow.AddYears(-65) : DateTime.UtcNow.AddYears(-30);
    }

    public IEnumerable<SimulateCreditRequest> Build(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new SimulateCreditRequest
            {
                LoanAmount = new LoanAmountDto { Amount = 1000, Currency = Currency.From("BRL") },
                Customer = new CustomerDetailsDto { BirthDate = _birth, Email = $"user{i}@test.com" },
                Months = 24,
                RateType = InterestRateType.Age,
                SourceCurrency = Currency.From("BRL"),
                TargetCurrency = Currency.From("BRL")
            };
        }
    }
}
