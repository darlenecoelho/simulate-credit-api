using SimulateCredit.Domain.ValueObjects;

namespace SimulateCredit.Application.Ports.Outgoing;

public interface ICurrencyConverterService
{
    /// <summary>
    /// Converts an amount from one currency to another async.
    /// </summary>
    /// <param name="amount">The monetary amount to convert.</param>
    /// <param name="fromCurrency">The source currency to convert from.</param>
    /// <param name="toCurrency">The target currency to convert to.</param>
    /// <returns>A task that represents the asynchronous operation and contains the converted amount.</returns>
    Task<decimal> ConvertAsync(decimal amount, Currency fromCurrency, Currency toCurrency);
}
