using System;
using Checkout.Interfaces;
using FluentAssertions;
using Ploeh.AutoFixture;
using Xunit;

namespace Checkout.Tests
{
    public class SkuPriceCalculatorTests
    {
        public Fixture AutoFixture { get; set; }
        public Random Random { get; set; }

        public SkuPriceCalculatorTests()
        {
            AutoFixture = new Fixture();
            Random = new Random((int)DateTime.UtcNow.Ticks);
        }

        [Fact]
        public void WhenCheckingIsCalculatingPriceForItem()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            ISkuPriceCalculator subject = new SkuPriceCalculator(item, expectedPrice);

            // Act
            // Assert
            subject.IsCalculatingPriceForItem(item).Should().Be(true);
        }

        [Fact]
        public void WhenCheckingIsNotCalculatingPriceForItem()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            ISkuPriceCalculator subject = new SkuPriceCalculator(item, expectedPrice);

            // Act
            // Assert
            var newItemCode = AutoFixture.Create<string>();
            subject.IsCalculatingPriceForItem(newItemCode).Should().Be(false);
        }

        [Fact]
        public void WhenCheckingIsCalculatingPriceForItemIgnoringCase()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            ISkuPriceCalculator subject = new SkuPriceCalculator(item.ToUpperInvariant(), expectedPrice);

            // Act
            // Assert
            subject.IsCalculatingPriceForItem(item.ToLowerInvariant()).Should().Be(true);
        }

        [Theory]
        [InlineData("A", 50, 3, 130, 1, 50)]
        [InlineData("A", 50, 3, 130, 2, 100)]
        [InlineData("A", 50, 3, 130, 3, 130)]
        [InlineData("A", 50, 3, 130, 4, 180)]
        [InlineData("A", 50, 3, 130, 6, 260)]
        [InlineData("A", 50, 3, 130, 7, 310)]
        [InlineData("B", 30, 2, 45, 1, 30)]
        [InlineData("B", 30, 2, 45, 2, 45)]
        [InlineData("B", 30, 2, 45, 3, 75)]
        [InlineData("B", 30, 2, 45, 4, 90)]
        [InlineData("B", 30, 2, 45, 5, 120)]
        [InlineData("C", 20, null, null, 1, 20)]
        [InlineData("C", 20, null, null, 2, 40)]
        [InlineData("C", 20, null, null, 4, 80)]
        [InlineData("C", 20, null, null, 8, 160)]
        [InlineData("D", 15, null, null, 1, 15)]
        [InlineData("D", 15, null, null, 2, 30)]
        [InlineData("D", 15, null, null, 3, 45)]
        [InlineData("D", 15, null, null, 5, 75)]
        public void WhenCalculatingThePriceOfItems(string item, int unitPrice, int? multiBuyItemCount,
            int? multiBuyPrice, int count, int expectedPrice)
        {
            // Arrange
            ISkuPriceCalculator subject = new SkuPriceCalculator(item, unitPrice, multiBuyItemCount, multiBuyPrice);

            // Act
            for (int i = 1; i < count; i++)
            {
                subject.IncrementItemCount();
            }

            // Assert
            subject.TotalPrice().Should().Be(expectedPrice);
        }
    }
}