using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.Ports.Outgoing;

public interface ICurrencyConverterService
{
    Task<decimal> ConvertAsync(decimal amount, Currency fromCurrency, Currency toCurrency);
}
