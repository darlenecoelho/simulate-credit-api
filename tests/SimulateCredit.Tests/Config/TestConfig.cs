using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using SimulateCredit.Application.Ports.Incoming;
using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Domain.Interfaces;

namespace SimulateCredit.Tests.Config;

public class TestConfig<TService> where TService : class
{
    public Mock<ILogger<TService>> _logger;
    public IConfigurationRoot _configuration;
    public Faker _faker;

    public Mock<ISimulateCreditUseCase> _simulateCreditUseCase;
    public Mock<ICurrencyConverterService> CurrencyConverterServiceMock;
    public Mock<IInterestStrategy> InterestStrategyMock;

    public TestConfig()
    {
        _faker = new Faker();

        ConfigLogger();
        ConfigServices();
        ConfigMocks();
    }

    private void ConfigLogger()
    {
        _logger = new Mock<ILogger<TService>>();
    }

    private void ConfigServices()
    {
        _configuration = new ConfigurationBuilder().Build();
    }

    private void ConfigMocks()
    {
        _simulateCreditUseCase = new Mock<ISimulateCreditUseCase>();
        CurrencyConverterServiceMock = new Mock<ICurrencyConverterService>();
        InterestStrategyMock = new Mock<IInterestStrategy>();
    }
}
