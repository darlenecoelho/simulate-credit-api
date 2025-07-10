using Bogus;
using FluentAssertions;
using Moq;
using SimulateCredit.Application.DTOs;
using SimulateCredit.Application.Factories;
using SimulateCredit.Application.Notifications;
using SimulateCredit.Application.Services;
using SimulateCredit.Application.UseCases.SimulateCredit;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Enums;
using SimulateCredit.Domain.ValueObjects;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Integration.UseCases;

public class SimulateCreditUseCaseIntegrationTests : TestConfig<SimulateCreditUseCase>
{
    private readonly SimulateCreditUseCase _useCase;

    public SimulateCreditUseCaseIntegrationTests()
    {
        CurrencyConverterServiceMock
            .Setup(x => x.ConvertAsync(
                It.IsAny<decimal>(),
                It.IsAny<Currency>(),
                It.IsAny<Currency>()))
            .ReturnsAsync((decimal v, Currency _, Currency __) => v);

        SimulationRepositoryMock
            .Setup(r => r.SaveAsync(It.IsAny<Simulation>()))
            .Returns(Task.CompletedTask);

        MediatorMock
            .Setup(m => m.Publish(
                It.IsAny<SimulationCompletedNotification>(),
                It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var calculator = new LoanCalculationService(CurrencyConverterServiceMock.Object);
        var factory = new SimulationFactoryAdapter();

        _useCase = new SimulateCreditUseCase(
            SimulationRepositoryMock.Object,
            MediatorMock.Object,
            calculator,
            factory,
            LoggerMock.Object);
    }

    [Theory]
    [InlineData(2001, 0.05)] // cliente com 24 anos = 5%
    [InlineData(1995, 0.03)] // cliente com 30 anos = 3%
    [InlineData(1975, 0.02)] // cliente com 50 anos = 2%
    public async Task ExecuteAsync_AgeBasedRates_CalculateCorrectly(int birthYear, double annualRate)
    {
        // Arrange
        decimal principal = 1000m;
        int months = 12;
        var request = new SimulateCreditRequest
        {
            LoanAmount = new LoanAmountDto { Amount = principal, Currency = Currency.From("BRL") },
            Customer = new CustomerDetailsDto { BirthDate = new DateTime(birthYear, 1, 1), Email = Faker.Internet.Email() },
            Months = months,
            RateType = InterestRateType.Age,
            SourceCurrency = Currency.From("BRL"),
            TargetCurrency = Currency.From("BRL")
        };

        // Act
        var response = await _useCase.ExecuteAsync(request);

        // Assert: taxa mensal = annualRate/12
        var r = (decimal)(annualRate / 12.0);
        var factor = (decimal)Math.Pow(1 + (double)r, months);
        var expectedMonthly = (principal * r * factor) / (factor - 1);

        response.MonthlyPayment.Should().Be(Math.Round(expectedMonthly, 2));

        // verifique que persistiu e notificou
        SimulationRepositoryMock.Verify(rp => rp.SaveAsync(It.IsAny<Simulation>()), Times.Once);
        MediatorMock.Verify(md => md.Publish(It.IsAny<SimulationCompletedNotification>(), It.IsAny<CancellationToken>()), Times.Once);

        // limpa para próximo caso
        SimulationRepositoryMock.Invocations.Clear();
        MediatorMock.Invocations.Clear();
    }

    [Fact]
    public async Task ExecuteAsync_VariableRate_AddsExtra1Point5Percent()
    {
        // Arrange: cliente 30 anos (3% a.a.) + 1.5% = 4.5%
        decimal principal = 1000m;
        int months = 12;
        var request = new SimulateCreditRequest
        {
            LoanAmount = new LoanAmountDto { Amount = principal, Currency = Currency.From("BRL") },
            Customer = new CustomerDetailsDto { BirthDate = DateTime.UtcNow.AddYears(-30), Email = Faker.Internet.Email() },
            Months = months,
            RateType = InterestRateType.AgeWithVariableRate,
            SourceCurrency = Currency.From("BRL"),
            TargetCurrency = Currency.From("BRL")
        };

        // Act
        var response = await _useCase.ExecuteAsync(request);

        // Assert: taxa mensal = 0.045/12
        var r = 0.045m / 12m;
        var factor = (decimal)Math.Pow(1 + (double)r, months);
        var expectedMonthly = (principal * r * factor) / (factor - 1);

        response.MonthlyPayment.Should().BeApproximately(Math.Round(expectedMonthly, 2), 0.01m);
    }
}