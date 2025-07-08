namespace SimulateCredit.Domain.ValueObjects;

public class Age
{
    public int Value { get; }

    public Age(DateTime birthDate)
    {
        var today = DateTime.Today;
        Value = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-Value)) Value--;
    }
}