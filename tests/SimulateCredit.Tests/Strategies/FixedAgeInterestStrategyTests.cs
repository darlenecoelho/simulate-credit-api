using FluentAssertions;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Strategies;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Strategies;

public class FixedInterestStrategyTests : TestConfig<FixedInterestStrategy>
{
    private static FixedInterestStrategy _methods => new();

    [Theory]
    [InlineData(24, 0.05)]
    [InlineData(30, 0.03)]
    [InlineData(50, 0.02)]
    [InlineData(70, 0.04)]
    public void GetAnnualRate_ShouldReturnCorrectRate(int age, decimal expectedRate)
    {
        // Arrange
        var customer = new Customer(DateTime.Today.AddYears(-age), "cliente@example.com");

        // Act
        var result = _methods.GetAnnualRate(customer);

        // Assert
        result.Should().Be(expectedRate);
    }

    [Fact]
    public void GetAnnualRate_ShouldThrow_WhenCustomerIsNull()
    {
        // Act
        var act = () => _methods.GetAnnualRate(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}