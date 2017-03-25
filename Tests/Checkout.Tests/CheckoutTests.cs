using FluentAssertions;
using Xunit;

namespace Checkout.Tests
{
    public class CheckoutIntegrationTests
    {
        [Theory]
        [InlineData("A", 50)]
        [InlineData("B", 30)]
        [InlineData("C", 20)]
        [InlineData("D", 15)]
        [InlineData("A;a;A", 130)]
        public void WhenScanningItemsThenTheTotalPriceShouldBeAsExpected(string items, int expectedTotal)
        {
            // Arrange
            ICheckout checkout = new Checkout();

            // Act
            var itemArray = items.Split(';');
            foreach (var item in itemArray)
            {
                checkout.Scan(item);

            }
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
            _totalPrice = item == "A" ? 50 : item == "B" ? 30 : item == "C" ? 20 : 15;
        }

        public int GetTotalPrice()
        {
            return _totalPrice;
        }
    }
}