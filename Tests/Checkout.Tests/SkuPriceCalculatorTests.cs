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

        public SkuPriceCalculatorTests()
        {
            AutoFixture = new Fixture();
        }

        [Fact]
        public void WhenCheckingIsCalculatingPriceForItem()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            ISkuPriceCalculator subject = new SkuPriceCalculator(item);

            // Act
            // Assert
            subject.IsCalculatingPriceForItem(item).Should().Be(true);
        }

        [Fact]
        public void WhenCheckingIsNotCalculatingPriceForItem()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            ISkuPriceCalculator subject = new SkuPriceCalculator(item);

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
            ISkuPriceCalculator subject = new SkuPriceCalculator(item.ToUpperInvariant());

            // Act
            // Assert
            subject.IsCalculatingPriceForItem(item.ToLowerInvariant()).Should().Be(true);
        }

        [Fact]
        public void WhenInitiallyNewedTheCalculatorReturnsTheExpectedPrice()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            var subject = new SkuPriceCalculator(item);

            // Act
            // Assert
            subject.TotalPrice().Should().Be(expectedPrice);
        }
    }

    public class SkuPriceCalculator : ISkuPriceCalculator
    {
        private readonly string _item;

        public SkuPriceCalculator(string item)
        {
            _item = item;
        }

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
            return string.Compare(_item, item, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}