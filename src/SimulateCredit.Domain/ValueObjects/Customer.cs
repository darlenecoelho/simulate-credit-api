using Flunt.Notifications;
using Flunt.Validations;

namespace SimulateCredit.Domain.ValueObjects;

public class Customer : Notifiable<Notification>
{
    public Customer(string name, string email, DateTime birthDate)
    {
        Name = name;
        Email = email;
        BirthDate = birthDate;

        AddNotifications(new Contract<Customer>()
            .Requires()
            .IsNotNullOrEmpty(name, nameof(Name), "Name is required.")
            .IsEmail(email, nameof(Email), "Invalid email.")
            .IsLowerOrEqualsThan(birthDate, DateTime.Today.AddYears(-18), nameof(BirthDate), "You must be at least 18 years old.")
        );
    }

    public string Name { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }

    public int GetAge()
    {
        var today = DateTime.Today;
        var age = today.Year - BirthDate.Year;
        if (BirthDate.Date > today.AddYears(-age)) age--;
        return age;
    }
}