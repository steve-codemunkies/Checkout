using Checkout.Data;
using Checkout.Data.Dto;
using Checkout.Exception;
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
        public void WhenBuildSkuPriceCalculatorForAMultiBuyItemACorrectlyPopulatedInstanceIsReturned()
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
            var result = subject.Build(skuWithMultiBuy.Item);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<SkuPriceCalculator>();
            result.TotalPrice().Should().Be(50);
            result.IncrementItemCount();
            result.IncrementItemCount();
            result.TotalPrice().Should().Be(130);
        }

        [Fact]
        public void WhenBuildSkuPriceCalculatorForANonMultiBuyItemACorrectlyPopulatedInstanceIsReturned()
        {
            // Arrange
            var skuWithMultiBuy = new SkuWithMultiBuy
            {
                Item = "C",
                UnitPrice = 20
            };
            ISkuPriceCalculatorFactory subject = Mocker.CreateInstance<SkuPriceCalculatorFactory>();
            Mocker.GetMock<IGetSkuWithMultiBuy>().Setup(get => get.Query(skuWithMultiBuy.Item)).Returns(skuWithMultiBuy);

            // Act
            var result = subject.Build(skuWithMultiBuy.Item);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<SkuPriceCalculator>();
            result.TotalPrice().Should().Be(20);
            result.IncrementItemCount();
            result.IncrementItemCount();
            result.TotalPrice().Should().Be(60);
        }

        [Fact]
        public void WhenBuildSkuPriceCalculatorForNonExistantItem()
        {
            // Arrange
            var expectedItem = "A";
            ISkuPriceCalculatorFactory subject = Mocker.CreateInstance<SkuPriceCalculatorFactory>();

            // Act
            var exception = Assert.Throws<SkuNotFoundException>(() => subject.Build(expectedItem));

            // Assert
            exception.Item.Should().Be(expectedItem);
        }
    }
}