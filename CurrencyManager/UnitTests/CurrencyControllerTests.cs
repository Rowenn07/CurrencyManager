using Xunit;
using Moq;
using CurrencyManager.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CurrencyManager.Tests
{
    public class CurrencyControllerTests
    {
        private readonly Mock<CurrencyService> _mockCurrencyService;
        private readonly Mock<ILogger<CurrencyController>> _mockLogger;
        private readonly CurrencyController _controller;

        public CurrencyControllerTests()
        {
            _mockCurrencyService = new Mock<CurrencyService>();
            _mockLogger = new Mock<ILogger<CurrencyController>>();
            _controller = new CurrencyController(_mockCurrencyService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task ConvertCurrency_ReturnsOkResult_WithConvertedAmount()
        {
            // Arrange
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100m;
            var convertedAmount = 89.99m;

            _mockCurrencyService.Setup(service => service.ConvertCurrency(baseCurrency, targetCurrency, amount))
                .ReturnsAsync(convertedAmount);

            // Act
            var result = await _controller.ConvertCurrency(baseCurrency, targetCurrency, amount);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(convertedAmount, okResult.Value);
        }

        [Fact]
        public async Task ConvertCurrency_ReturnsBadRequest_WhenBaseCurrencyIsMissing()
        {
            // Act
            var result = await _controller.ConvertCurrency(null, "EUR", 100m);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Base currency and target currency must be provided.", badRequestResult.Value);
        }

        [Fact]
        public async Task ConvertCurrency_ReturnsInternalServerError_OnException()
        {
            // Arrange
            var baseCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100m;

            _mockCurrencyService.Setup(service => service.ConvertCurrency(baseCurrency, targetCurrency, amount))
                .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.ConvertCurrency(baseCurrency, targetCurrency, amount);

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while converting currency.", internalServerErrorResult.Value);
        }

        [Fact]
        public async Task GetConversionHistory_ReturnsOkResult_WithHistory()
        {
            // Arrange
            var history = new List<ConversionHistory>
            {
                new ConversionHistory { BaseCurrency = "USD", TargetCurrency = "EUR", Amount = 100m, ConvertedAmount = 89.99m, Timestamp = DateTime.UtcNow }
            };

            _mockCurrencyService.Setup(service => service.GetConversionHistory())
                .ReturnsAsync(history);

            // Act
            var result = await _controller.GetConversionHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
            Assert.Equal(history, okResult.Value);
        }

        [Fact]
        public async Task GetConversionHistory_ReturnsInternalServerError_OnException()
        {
            // Arrange
            _mockCurrencyService.Setup(service => service.GetConversionHistory())
                .ThrowsAsync(new System.Exception("Test exception"));

            // Act
            var result = await _controller.GetConversionHistory();

            // Assert
            var internalServerErrorResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.Equal("An error occurred while retrieving conversion history.", internalServerErrorResult.Value);
        }
    }
}
