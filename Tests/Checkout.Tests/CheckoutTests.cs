using FluentAssertions;
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

    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }

    public class Checkout : ICheckout
    {
        public void Scan(string item)
        {
            throw new System.NotImplementedException();
        }

        public int GetTotalPrice()
        {
            throw new System.NotImplementedException();
        }
    }
}