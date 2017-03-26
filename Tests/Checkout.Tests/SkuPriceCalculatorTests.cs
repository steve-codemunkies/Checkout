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

        [Fact]
        public void WhenInitiallyNewedTheCalculatorReturnsTheExpectedPrice()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            var subject = new SkuPriceCalculator(item, expectedPrice);

            // Act
            // Assert
            subject.TotalPrice().Should().Be(expectedPrice);
        }

        [Fact]
        public void WhenCalculatingThePriceForTwoItemsItShouldReturnTheCorrectPrice()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            var subject = new SkuPriceCalculator(item, expectedPrice);

            // Act
            subject.IncrementItemCount();

            // Assert
            subject.TotalPrice().Should().Be(expectedPrice * 2);
        }

        [Fact]
        public void WhenCalculatingThePriceForNItemsItShouldReturnTheCorrectPrice()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            var itemCount = Random.Next(1, 11);
            var subject = new SkuPriceCalculator(item, expectedPrice);

            // Act
            for (var i = 1; i < itemCount; i++)
            {
                subject.IncrementItemCount();
            }

            // Assert
            subject.TotalPrice().Should().Be(expectedPrice * itemCount);
        }

        [Fact]
        public void WhenCalculatingThePriceOf3ItemsWithAMultiBuySpecialOffer()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var singlePrice = 50;
            var multiBuyItemCount = 3;
            var multiBuyPrice = 130;
            var subject = new SkuPriceCalculator(item, singlePrice, multiBuyItemCount, multiBuyPrice);

            // Act
            subject.IncrementItemCount();
            subject.IncrementItemCount();

            // Assert
            subject.TotalPrice().Should().Be(multiBuyPrice);
        }

        [Fact]
        public void WhenCalculatingThePriceOf4ItemsWithAMultiBuySpecialOffer()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var singlePrice = 50;
            var multiBuyItemCount = 3;
            var multiBuyPrice = 130;
            var subject = new SkuPriceCalculator(item, singlePrice, multiBuyItemCount, multiBuyPrice);

            // Act
            subject.IncrementItemCount();
            subject.IncrementItemCount();
            subject.IncrementItemCount();

            // Assert
            subject.TotalPrice().Should().Be(singlePrice + multiBuyPrice);
        }

        [Fact]
        public void WhenCalculatingThePriceOf6ItemsWithAMultiBuySpecialOffer()
        {
            // Arrange
            var item = AutoFixture.Create<string>();
            var singlePrice = 50;
            var multiBuyItemCount = 3;
            var multiBuyPrice = 130;
            var subject = new SkuPriceCalculator(item, singlePrice, multiBuyItemCount, multiBuyPrice);

            // Act
            subject.IncrementItemCount();
            subject.IncrementItemCount();
            subject.IncrementItemCount();
            subject.IncrementItemCount();
            subject.IncrementItemCount();

            // Assert
            subject.TotalPrice().Should().Be(multiBuyPrice * 2);
        }
    }

    public class SkuPriceCalculator : ISkuPriceCalculator
    {
        private int _count = 1;
        private readonly string _item;
        private readonly int _unitPrice;
        private readonly int? _multiBuyItemCount;
        private readonly int? _multiBuyPrice;

        public SkuPriceCalculator(string item, int unitPrice, int? multiBuyItemCount = null, int? multiBuyPrice = null)
        {
            _item = item;
            _unitPrice = unitPrice;
            _multiBuyItemCount = multiBuyItemCount;
            _multiBuyPrice = multiBuyPrice;
        }

        public int TotalPrice()
        {
            if (_multiBuyItemCount.HasValue && _multiBuyPrice.HasValue && _count >= _multiBuyItemCount.Value)
            {
                return (_count / _multiBuyItemCount.Value) * _multiBuyPrice.Value;
            }

            return _unitPrice * _count;
        }

        public void IncrementItemCount()
        {
            _count++;
        }

        public bool IsCalculatingPriceForItem(string item)
        {
            return string.Compare(_item, item, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}