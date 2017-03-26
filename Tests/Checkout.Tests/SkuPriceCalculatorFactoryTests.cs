using Checkout.Interfaces;
using FluentAssertions;
using Xunit;

namespace Checkout.Tests
{
    public class SkuPriceCalculatorFactoryTests
    {
        [Fact]
        public void WhenBuildSkuPriceCalculatorACorrectlyPopulatedInstanceIsReturned()
        {
            // Arrange
            ISkuPriceCalculatorFactory subject = new SkuPriceCalculatorFactory();

            // Act
            var result = subject.Build("A");

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<SkuPriceCalculator>();
            result.TotalPrice().Should().Be(50);
            result.IncrementItemCount();
            result.IncrementItemCount();
            result.TotalPrice().Should().Be(130);
        }
    }

    public class SkuPriceCalculatorFactory : ISkuPriceCalculatorFactory
    {
        public ISkuPriceCalculator Build(string item)
        {
            throw new System.NotImplementedException();
        }
    }
}