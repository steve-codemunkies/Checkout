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
    }
}