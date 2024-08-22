namespace CurrencyManager.Models
{
    public class RateData
    {
        /// <summary>
        /// Gets or sets the dictionary of currency rates.
        /// </summary>
        public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
    }
}