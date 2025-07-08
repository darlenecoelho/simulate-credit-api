namespace SimulateCredit.Domain.ValueObjects;

public class Currency
{
    public Currency(string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length != 3)
            throw new ArgumentException("Invalid currency code");

        Code = code.ToUpperInvariant();
    }
    public string Code { get; }
    public bool IsBRL => Code == "BRL";
    public bool IsUSD => Code == "USD";
    public bool IsMXN => Code == "MXN";
}
