using FluentAssertions;
using Xunit;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        [Theory]
        [InlineData("A", 50)]
        [InlineData("B", 30)]
        [InlineData("C", 20)]
        [InlineData("D", 15)]
        public void WhenScanningAnItemThenTheTotalPriceShouldBeAsExpected(string item, int expectedTotal)
        {
            // Arrange
            ICheckout checkout = new Checkout();

            // Act
            checkout.Scan(item);
            var result = checkout.GetTotalPrice();

            // Assert
            result.Should().Be(expectedTotal);
        }
    }

    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }

    public class Checkout : ICheckout
    {
        private int _totalPrice;

        public void Scan(string item)
        {
            _totalPrice = item == "A" ? 50 : item == "B" ? 30 : 20;
        }

        public int GetTotalPrice()
        {
            return _totalPrice;
        }
    }
}