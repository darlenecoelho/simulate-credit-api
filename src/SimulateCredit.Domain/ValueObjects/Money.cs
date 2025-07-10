namespace SimulateCredit.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative.");
        Amount = amount;
    }
}
