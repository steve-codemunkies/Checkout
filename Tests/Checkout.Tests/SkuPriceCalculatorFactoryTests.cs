using Checkout.Interfaces;
using FluentAssertions;
using Moq.AutoMock;
using Xunit;

namespace Checkout.Tests
{
    public class SkuPriceCalculatorFactoryTests
    {
        public AutoMocker Mocker { get; set; }

        public SkuPriceCalculatorFactoryTests()
        {
            Mocker = new AutoMocker();
        }

        [Fact]
        public void WhenBuildSkuPriceCalculatorACorrectlyPopulatedInstanceIsReturned()
        {
            // Arrange
            var skuWithMultiBuy = new SkuWithMultiBuy
            {
                Item = "A",
                UnitPrice = 50,
                MultiBuyItemCount = 3,
                MultiBuyPrice = 130
            };
            ISkuPriceCalculatorFactory subject = Mocker.CreateInstance<SkuPriceCalculatorFactory>();
            Mocker.GetMock<IGetSkuWithMultiBuy>().Setup(get => get.Query(skuWithMultiBuy.Item)).Returns(skuWithMultiBuy);

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

    public interface IGetSkuWithMultiBuy
    {
        SkuWithMultiBuy Query(string item);
    }

    public class SkuWithMultiBuy
    {
        public string Item { get; set; }
        public int UnitPrice { get; set; }
        public int MultiBuyItemCount { get; set; }
        public int MultiBuyPrice { get; set; }
    }

    public class SkuPriceCalculatorFactory : ISkuPriceCalculatorFactory
    {
        private readonly IGetSkuWithMultiBuy _getSkuWithMultiBuy;

        public SkuPriceCalculatorFactory(IGetSkuWithMultiBuy getSkuWithMultiBuy)
        {
            _getSkuWithMultiBuy = getSkuWithMultiBuy;
        }

        public ISkuPriceCalculator Build(string item)
        {
            var skuWithMultiBuy = _getSkuWithMultiBuy.Query(item);
            return new SkuPriceCalculator(skuWithMultiBuy.Item, skuWithMultiBuy.UnitPrice,
                skuWithMultiBuy.MultiBuyItemCount, skuWithMultiBuy.MultiBuyPrice);
        }
    }
}