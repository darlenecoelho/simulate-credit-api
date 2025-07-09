using System.Text.RegularExpressions;

namespace SimulateCredit.Domain.ValueObjects;

public sealed class Currency : IEquatable<Currency>
{
    private static readonly Regex Iso4217Regex = new("^[A-Z]{3}$", RegexOptions.Compiled);

    private Currency(string code)
    {
        Code = code.ToUpperInvariant();
    }
    public string Code { get; }

    public static Currency From(string code)
    {
        if (!IsValid(code))
            throw new ArgumentException("Invalid currency code.");

        return new Currency(code);
    }

    public static bool IsValid(string code) =>
        !string.IsNullOrWhiteSpace(code) && Iso4217Regex.IsMatch(code.ToUpperInvariant());

    public override string ToString() => Code;

    public override bool Equals(object? obj) => obj is Currency other && Equals(other);
    public bool Equals(Currency? other) => other != null && Code == other.Code;
    public override int GetHashCode() => Code.GetHashCode();

    public static implicit operator string(Currency currency) => currency.Code;
}
