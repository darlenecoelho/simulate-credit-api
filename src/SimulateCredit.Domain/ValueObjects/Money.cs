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

    public static Money operator +(Money a, Money b) => new(a.Amount + b.Amount);
    public static Money operator *(Money a, decimal b) => new(a.Amount * b);
    public static Money operator *(decimal b, Money a) => new(a.Amount * b);
    public static implicit operator decimal(Money m) => m.Amount;

    public override string ToString() => Amount.ToString("C2");
}
