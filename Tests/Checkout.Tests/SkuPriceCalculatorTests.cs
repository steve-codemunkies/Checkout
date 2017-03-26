﻿using System;
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
    }

    public class SkuPriceCalculator : ISkuPriceCalculator
    {
        private readonly string _item;
        private readonly int _unitPrice;

        public SkuPriceCalculator(string item, int unitPrice)
        {
            _item = item;
            _unitPrice = unitPrice;
        }

        public int TotalPrice()
        {
            return _unitPrice;
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