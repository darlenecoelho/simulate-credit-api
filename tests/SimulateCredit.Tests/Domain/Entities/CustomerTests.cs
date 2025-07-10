using FluentAssertions;
using SimulateCredit.Domain.Entities;
using SimulateCredit.Domain.Enums;
using SimulateCredit.Tests.Config;

namespace SimulateCredit.Tests.Domain.Entities;

public class CustomerTests : TestConfig<Customer>
{
    [Fact]
    public void Constructor_ShouldSetBirthDateAndEmail()
    {
        // Arrange
        var birthDate = new DateTime(1995, 5, 10);
        var email = "teste@test.com";

        // Act
        var customer = new Customer(birthDate, email);

        // Assert
        customer.BirthDate.Should().Be(birthDate);
        customer.Email.Should().Be(email);
    }

    [Theory]
    [InlineData("2007-01-01", InterestAgeGroup.UP_TO_25)]
    [InlineData("1995-01-01", InterestAgeGroup.FROM_26_TO_40)]
    [InlineData("1980-01-01", InterestAgeGroup.FROM_41_TO_60)]
    [InlineData("1950-01-01", InterestAgeGroup.ABOVE_60)]
    public void GetInterestRateGroup_ShouldReturnCorrectGroup(string birthDateString, InterestAgeGroup expectedGroup)
    {
        // Arrange
        var birthDate = DateTime.Parse(birthDateString);
        var customer = new Customer(birthDate, "test@test.com");

        // Act
        var result = customer.GetInterestRateGroup();

        // Assert
        result.Should().Be(expectedGroup);
    }

    [Theory]
    [InlineData("2000-07-08")]
    [InlineData("1985-03-22")]
    [InlineData("1960-11-30")]
    public void GetAge_ShouldCalculateCorrectly(string birthDateString)
    {
        // Arrange
        var birthDate = DateTime.Parse(birthDateString);
        var expectedAge = CalculateExpectedAge(birthDate);
        var customer = new Customer(birthDate, "customer@test.com");

        // Act
        var age = customer.GetAge();

        // Assert
        age.Should().Be(expectedAge);
    }

    private int CalculateExpectedAge(DateTime birthDate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age)) age--;
        return age;
    }
}
