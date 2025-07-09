using FluentAssertions;
using SimulateCredit.Domain.ValueObjects;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Domain.ValueObjects;

public class CurrencyTests : TestConfig<Currency>
{
    [Theory]
    [InlineData("BRL", true)]
    [InlineData("USD", true)]
    [InlineData("eur", true)]
    [InlineData("invalid", false)]
    [InlineData("", false)]
    public void IsValid_ShouldReturnExpected(string input, bool expected)
    {
        Currency.IsValid(input).Should().Be(expected);
    }

    [Fact]
    public void From_ShouldThrowException_WhenInvalid()
    {
        var act = () => Currency.From("invalid");
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void From_ShouldReturnCurrency_WhenValid()
    {
        var currency = Currency.From("usd");
        currency.Code.Should().Be("USD");
    }
}
