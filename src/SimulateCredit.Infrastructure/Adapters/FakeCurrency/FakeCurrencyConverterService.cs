using SimulateCredit.Application.Ports.Outgoing;
using SimulateCredit.Domain.ValueObjects;
namespace SimulateCredit.Infrastructure.Adapters.FakeCurrency;

public sealed class FakeCurrencyConverterService : ICurrencyConverterService
{
    private readonly IAuditLogger _auditLogger;

    public FakeCurrencyConverterService(IAuditLogger auditLogger)
        => _auditLogger = auditLogger;

    public Task<decimal> ConvertAsync(decimal amount, Currency fromCurrency, Currency toCurrency)
    {
        _auditLogger.LogInformation("Converting {Amount} from {From} to {To}", amount, fromCurrency, toCurrency);

        try
        {
            if (fromCurrency.Equals(toCurrency))
                return Task.FromResult(amount);

            return (fromCurrency.Code, toCurrency.Code) switch
            {
                ("BRL", "USD") => Task.FromResult(amount / 5m),
                ("BRL", "EUR") => Task.FromResult(amount / 6m),
                ("USD", "BRL") => Task.FromResult(amount * 5m),
                ("EUR", "BRL") => Task.FromResult(amount * 6m),
                _ => throw new NotSupportedException($"Conversion from {fromCurrency} to {toCurrency} is not supported.")
            };
        }
        catch (Exception ex)
        {
            _auditLogger.LogError(ex, "Error converting currency");
            throw;
        }
    }
}