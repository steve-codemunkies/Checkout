﻿using FluentAssertions;
using Xunit;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        [Fact]
        public void WhenScanningOneItemAThenTheTotalPriceShouldBeFifty()
        {
            // Arrange
            ICheckout checkout = new Checkout();

            // Act
            checkout.Scan("A");
            var result = checkout.GetTotalPrice();

            // Assert
            result.Should().Be(50);
        }

        [Fact]
        public void WhenScanningOneItemBThenTheTotalPriceShouldBeThirty()
        {
            // Arrange
            ICheckout checkout = new Checkout();

            // Act
            checkout.Scan("B");
            var result = checkout.GetTotalPrice();

            // Assert
            result.Should().Be(30);
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
            _totalPrice = item == "A" ? 50 : 30;
        }

        public int GetTotalPrice()
        {
            return _totalPrice;
        }
    }
}