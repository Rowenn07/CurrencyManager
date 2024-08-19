using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

/// <summary>
/// Interface for currency conversion services.
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    /// <param name="baseCurrency">The currency to convert from.</param>
    /// <param name="targetCurrency">The currency to convert to.</param>
    /// <param name="amount">The amount to convert.</param>
    /// <returns>The converted amount as a decimal.</returns>
    Task<decimal> ConvertCurrency(string baseCurrency, string targetCurrency, decimal amount);

    /// <summary>
    /// Retrieves the history of currency conversions.
    /// </summary>
    /// <returns>A collection of conversion history records.</returns>
    Task<IEnumerable<ConversionHistory>> GetConversionHistory();
}
