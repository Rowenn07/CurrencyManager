using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

/// <summary>
/// Service for handling currency conversions.
/// </summary>
public class CurrencyService : ICurrencyService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IDistributedCache _cache;
    private readonly MyDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="CurrencyService"/> class.
    /// </summary>
    /// <param name="httpClientFactory">The HTTP client factory.</param>
    /// <param name="cache">The distributed cache.</param>
    /// <param name="context">The database context.</param>
    public CurrencyService(IHttpClientFactory httpClientFactory, IDistributedCache cache, MyDbContext context)
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache;
        _context = context;
    }

    /// <summary>
    /// Converts an amount from one currency to another.
    /// </summary>
    /// <param name="baseCurrency">The currency to convert from.</param>
    /// <param name="targetCurrency">The currency to convert to.</param>
    /// <param name="amount">The amount to convert.</param>
    /// <returns>The converted amount as a decimal.</returns>
    public async Task<decimal> ConvertCurrency(string baseCurrency, string targetCurrency, decimal amount)
    {
        // Check if the exchange rates are in the cache.
        var cacheKey = "exchange_rates";
        var cachedRates = await _cache.GetStringAsync(cacheKey);
        RateData rateData = null;

        if (!string.IsNullOrEmpty(cachedRates))
        {
            rateData = JsonConvert.DeserializeObject<RateData>(cachedRates);
        }

        if (rateData == null)
        {
            // Fetch the latest exchange rates from the API.
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetStringAsync("https://openexchangerates.org/api/latest.json?app_id=7b858eb7976442abb4497106a55f0457");
            rateData = JsonConvert.DeserializeObject<RateData>(response);

            // Cache the exchange rates for 15 minutes.
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15)
            };
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(rateData), cacheOptions);
        }

        if (rateData == null || !rateData.Rates.ContainsKey(baseCurrency) || !rateData.Rates.ContainsKey(targetCurrency))
        {
            throw new InvalidOperationException("Exchange rates data is not available.");
        }

        // Calculate the conversion rate.
        var baseRate = rateData.Rates[baseCurrency];
        var targetRate = rateData.Rates[targetCurrency];
        var conversionRate = targetRate / baseRate;

        // Create a new conversion history record.
        var conversion = new ConversionHistory
        {
            BaseCurrency = baseCurrency,
            TargetCurrency = targetCurrency,
            Amount = amount,
            ConvertedAmount = amount * conversionRate,
            Timestamp = DateTime.UtcNow
        };

        try
        {
            // Save the conversion history to the database.
            _context.ConversionHistory.Add(conversion);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Log the exception details.
            Console.WriteLine($"An error occurred while saving the conversion history: {ex.Message}");

            throw;
        }

        return conversion.ConvertedAmount;
    }

    /// <summary>
    /// Retrieves the history of currency conversions.
    /// </summary>
    /// <returns>A collection of conversion history records.</returns>
    public async Task<IEnumerable<ConversionHistory>> GetConversionHistory()
    {
        return await _context.ConversionHistory.ToListAsync();
    }
}

/// <summary>
/// Represents the exchange rate data.
/// </summary>
public class RateData
{
    /// <summary>
    /// Gets or sets the dictionary of currency rates.
    /// </summary>
    public Dictionary<string, decimal> Rates { get; set; }
}
   