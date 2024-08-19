using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CurrencyManager.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurrencyController : ControllerBase
    {
        private readonly CurrencyService _currencyService;
        private readonly ILogger<CurrencyController> _logger;

        public CurrencyController(CurrencyService currencyService, ILogger<CurrencyController> logger)
        {
            _currencyService = currencyService;
            _logger = logger;
        }

        /// <summary>
        /// Converts an amount from one currency to another.
        /// </summary>
        /// <param name="baseCurrency">The base currency code.</param>
        /// <param name="targetCurrency">The target currency code.</param>
        /// <param name="amount">The amount to convert.</param>
        /// <returns>The converted amount.</returns>
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string baseCurrency, [FromQuery] string targetCurrency, [FromQuery] decimal amount)
        {
            try
            {
                if (string.IsNullOrEmpty(baseCurrency) || string.IsNullOrEmpty(targetCurrency))
                {
                    return BadRequest("Base currency and target currency must be provided.");
                }

                var result = await _currencyService.ConvertCurrency(baseCurrency, targetCurrency, amount);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument provided for currency conversion: {BaseCurrency} to {TargetCurrency}", baseCurrency, targetCurrency);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while converting currency from {BaseCurrency} to {TargetCurrency}", baseCurrency, targetCurrency);
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while converting currency.");
            }
        }

        /// <summary>
        /// Retrieves the conversion history.
        /// </summary>
        /// <returns>A list of conversion history records.</returns>
        [HttpGet("history")]
        public async Task<IActionResult> GetConversionHistory()
        {
            try
            {
                var history = await _currencyService.GetConversionHistory();
                return Ok(history);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving conversion history");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occurred while retrieving conversion history.");
            }
        }
    }
}