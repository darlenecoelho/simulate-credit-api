using SimulateCredit.Domain.Enums;

namespace SimulateCredit.Domain.Entities;

public sealed class Customer
{
    public Customer(DateTime birthDate, string email)
    {
        BirthDate = birthDate;
        Email = email;
    }

    public DateTime BirthDate { get; }
    public string Email { get; }

    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - BirthDate.Year;
        if (BirthDate > today.AddYears(-age)) age--;
        return age;
    }

    public InterestAgeGroup GetInterestRateGroup()
    {
        var age = GetAge();
        return age switch
        {
            <= 25 => InterestAgeGroup.UP_TO_25,
            <= 40 => InterestAgeGroup.FROM_26_TO_40,
            <= 60 => InterestAgeGroup.FROM_41_TO_60,
            _ => InterestAgeGroup.ABOVE_60
        };
    }
}