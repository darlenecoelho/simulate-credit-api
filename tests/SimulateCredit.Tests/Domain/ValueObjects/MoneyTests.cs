using FluentAssertions;
using SimulateCredit.Domain.ValueObjects;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Domain.ValueObjects;

public class MoneyTests : TestConfig<Money>
{
    [Fact]
    public void Constructor_WithNegativeAmount_ShouldThrowException()
    {
        // Arrange / Act
        var act = () => new Money(-100);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenAmountsAreEqual()
    {
        var m1 = new Money(100);
        var m2 = new Money(100);

        m1.Should().BeEquivalentTo(m2);
    }
}
