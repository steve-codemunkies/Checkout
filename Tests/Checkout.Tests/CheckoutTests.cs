using Xunit;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void WhenScanningOneItemAThenTheTotalPriceShouldBeFifty()
        {
            // Arrange
            ICheckout checkout = new Checkout();

            // Act
            checkout.Scan("A");
            var result = checkout.GetTotalPrice();

            // Assert
            result.Should().Be(50);
        }
    }
}