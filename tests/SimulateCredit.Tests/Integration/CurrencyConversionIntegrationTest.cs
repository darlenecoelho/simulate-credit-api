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

namespace SimulateCredit.Tests.Integration.UseCases
{
    public class CurrencyConversionIntegrationTest : TestConfig<SimulateCreditUseCase>
    {
        private readonly SimulateCreditUseCase _useCase;

        public CurrencyConversionIntegrationTest()
        {
            CurrencyConverterServiceMock
                .Setup(x => x.ConvertAsync(
                    It.IsAny<decimal>(),
                    It.IsAny<Currency>(),
                    It.IsAny<Currency>()))
                .ReturnsAsync((decimal v, Currency a, Currency b) => v);

            CurrencyConverterServiceMock
                .Setup(x => x.ConvertAsync(
                    It.IsAny<decimal>(),
                    Currency.From("USD"),
                    Currency.From("BRL")))
                .ReturnsAsync((decimal v, Currency a, Currency b) => v * 5m);

            SimulationRepositoryMock
                .Setup(r => r.SaveAsync(It.IsAny<Simulation>()))
                .Returns(Task.CompletedTask);

            MediatorMock
                .Setup(m => m.Publish(
                    It.IsAny<SimulationCompletedNotification>(),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var calculator = new LoanCalculationService(
                CurrencyConverterServiceMock.Object);

            var factory = new SimulationFactoryAdapter();

            _useCase = new SimulateCreditUseCase(
                simulationRepository: SimulationRepositoryMock.Object,
                mediator: MediatorMock.Object,
                calculator: calculator,
                factory: factory);
        }

        [Fact]
        public async Task ExecuteAsync_USDToBRLConversion_AppliesConversionBeforeCalculation()
        {
            // Arrange: 100 USD →=500 BRL
            var request = new SimulateCreditRequest
            {
                LoanAmount = new LoanAmountDto { Amount = 100m, Currency = Currency.From("USD") },
                Customer = new CustomerDetailsDto { BirthDate = DateTime.UtcNow.AddYears(-30), Email = Faker.Internet.Email() },
                Months = 12,
                RateType = InterestRateType.Age,
                SourceCurrency = Currency.From("USD"),
                TargetCurrency = Currency.From("BRL")
            };

            // Act
            var response = await _useCase.ExecuteAsync(request);

            // Assert: principal usado = 100 * 5 = 500
            var usedPrincipal = 100m * 5m;
            var monthlyRate = 0.03m / 12m;
            var factor = (decimal)Math.Pow(1 + (double)monthlyRate, request.Months);
            var expectedMonthly = (usedPrincipal * monthlyRate * factor) / (factor - 1);

            response.MonthlyPayment
                .Should().BeApproximately(Math.Round(expectedMonthly, 2), 0.01m);
        }
    }
}
