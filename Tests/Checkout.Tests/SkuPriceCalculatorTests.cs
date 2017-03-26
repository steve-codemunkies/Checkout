using Checkout.Interfaces;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace Checkout.Tests
{
    public class SkuPriceCalculatorTests
    {
        public Fixture AutoFixture { get; set; }

        public SkuPriceCalculatorTests()
        {
            AutoFixture = new Fixture();
        }

        [Fact]
        public void WhenCheckingIsCalculatingPriceForItem()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            ISkuPriceCalculator subject = new SkuPriceCalculator();

            // Act
            // Assert
            subject.IsCalculatingPriceForItem(item).Should().Be(true);
        }

        [Fact]
        public void WhenCheckingIsNotCalculatingPriceForItem()
        {
            // Arrange
            ISkuPriceCalculator subject = new SkuPriceCalculator();

            // Act
            // Assert
            var item = AutoFixture.Create<string>();
            subject.IsCalculatingPriceForItem(item).Should().Be(false);
        }
    }

    public class SkuPriceCalculator : ISkuPriceCalculator
    {
        public int TotalPrice()
        {
            throw new System.NotImplementedException();
        }

        public void IncrementItemCount()
        {
            throw new System.NotImplementedException();
        }

        public bool IsCalculatingPriceForItem(string item)
        {
            return true;
        }
    }
}