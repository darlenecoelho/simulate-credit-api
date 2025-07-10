using Bogus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SimulateCredit.Application.Interfaces;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Domain.Interfaces;

namespace SimulateCredit.Tests.Config;

public class TestConfig<TService> where TService : class
{
    public Mock<ILogger<TService>> LoggerMock { get; }
    public IConfigurationRoot Configuration { get; }
    public Faker Faker { get; }

    public Mock<ICurrencyConverterService> CurrencyConverterServiceMock { get; }
    public Mock<IInterestStrategy> InterestStrategyMock { get; }
    public Mock<ISimulationRepository> SimulationRepositoryMock { get; }
    public Mock<IMediator> MediatorMock { get; }
    public Mock<ICreditSimulationPublisher> CreditSimulationPublisherMock { get; }
    public Mock<INotificationService> NotificationServiceMock { get; }
    public Mock<ISimulationFactory> SimulationFactoryMock { get; }

    public TestConfig()
    {
        LoggerMock = new Mock<ILogger<TService>>();
        Configuration = new ConfigurationBuilder().Build();
        Faker = new Faker();

        CurrencyConverterServiceMock = new Mock<ICurrencyConverterService>();
        InterestStrategyMock = new Mock<IInterestStrategy>();
        SimulationRepositoryMock = new Mock<ISimulationRepository>();
        MediatorMock = new Mock<IMediator>();
        CreditSimulationPublisherMock = new Mock<ICreditSimulationPublisher>();
        NotificationServiceMock = new Mock<INotificationService>();
        SimulationFactoryMock = new Mock<ISimulationFactory>();
    }
}