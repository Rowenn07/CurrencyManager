using Microsoft.EntityFrameworkCore;

/// <summary>
/// Represents a record of a currency conversion.
/// </summary>
public class ConversionHistory
{
    /// <summary>
    /// Gets or sets the unique identifier for the conversion history record.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the base currency code.
    /// </summary>
    public string BaseCurrency { get; set; }

    /// <summary>
    /// Gets or sets the target currency code.
    /// </summary>
    public string TargetCurrency { get; set; }

    /// <summary>
    /// Gets or sets the amount in the base currency.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Gets or sets the converted amount in the target currency.
    /// </summary>
    public decimal ConvertedAmount { get; set; }

    /// <summary>
    /// Gets or sets the timestamp of the conversion.
    /// </summary>
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Database context for the application.
/// </summary>
public class MyDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

    /// <summary>
    /// Gets or sets the DbSet for conversion history records.
    /// </summary>
    public DbSet<ConversionHistory> ConversionHistories { get; set; }
}
   