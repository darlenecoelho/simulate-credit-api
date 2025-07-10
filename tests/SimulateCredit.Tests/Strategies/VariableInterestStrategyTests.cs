using FluentAssertions;
using Moq;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Interfaces;
using SimulateCredit.Domain.Strategies;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Strategies;

public class VariableInterestStrategyTests : TestConfig<VariableInterestStrategy>
{
    [Theory]
    [InlineData(24, 0.05, 0.015, 0.065)]
    [InlineData(30, 0.03, 0.015, 0.045)]
    public void GetAnnualRate_ShouldIncludeExtraRate(int age, decimal baseRate, decimal extra, decimal expected)
    {
        // Arrange
        var customer = new Customer(DateTime.Today.AddYears(-age), "customer@test.com");
        var baseStrategy = new FakeFixedInterestStrategy(baseRate);
        var service = new VariableInterestStrategy(baseStrategy, extra);

        // Act
        var rate = service.GetAnnualRate(customer);

        // Assert
        rate.Should().Be(expected);
    }

    [Fact]
    public void GetAnnualRate_ShouldAddExtraRateToBase_WhenCustomerIsValid()
    {
        // Arrange
        var customer = new Customer(new DateTime(1995, 7, 8), "customer@test.com");
        var mockBaseStrategy = new Mock<IInterestStrategy>();
        mockBaseStrategy.Setup(s => s.GetAnnualRate(customer)).Returns(0.03m);

        var strategy = new VariableInterestStrategy(mockBaseStrategy.Object, 0.015m);

        // Act
        var result = strategy.GetAnnualRate(customer);

        // Assert
        result.Should().Be(0.045m);
    }

    [Fact]
    public void GetAnnualRate_ShouldThrow_WhenCustomerIsNull()
    {
        // Arrange
        var mockBaseStrategy = new Mock<IInterestStrategy>();
        var strategy = new VariableInterestStrategy(mockBaseStrategy.Object, 0.01m);

        // Act
        var act = () => strategy.GetAnnualRate(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    private class FakeFixedInterestStrategy : IInterestStrategy
    {
        private readonly decimal _rate;
        public FakeFixedInterestStrategy(decimal rate) => _rate = rate;
        public decimal GetAnnualRate(Customer _) => _rate;
    }
}
