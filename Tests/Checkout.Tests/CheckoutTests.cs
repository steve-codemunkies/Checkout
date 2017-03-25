using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Ploeh.AutoFixture;
using Xunit;

namespace Checkout.Tests
{
    public class CheckoutTests
    {
        public Fixture AutoFixture { get; set; }
        public AutoMocker Mocker { get; set; }
        public Random Random { get; set; }

        public CheckoutTests()
        {
            AutoFixture = new Fixture();
            Mocker = new AutoMocker();
            Random = new Random((int)DateTime.UtcNow.Ticks);
        }

        [Fact]
        public void WhenScanningAUniqueItemThenThePriceIsTheExpectedPrice()
        {
            // Arrange
            ICheckout subject = Mocker.CreateInstance<Checkout>();

            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();

            var calculator = Mocker.GetMock<ISkuPriceCalculator>();
            calculator.Setup(calc => calc.TotalPrice()).Returns(expectedPrice);
            Mocker.GetMock<ISkuPriceCalculatorFactory>()
                .Setup(factory => factory.Build(item))
                .Returns(calculator.Object);

            // Act
            subject.Scan(item);
            var result = subject.GetTotalPrice();

            // Assert
            result.Should().Be(expectedPrice);
        }

        [Fact]
        public void WhenScanningTwoUniqueItemThenThePriceIsTheExpectedPrice()
        {
            // Arrange
            ICheckout subject = Mocker.CreateInstance<Checkout>();

            var item1 = AutoFixture.Create<string>();
            var expectedPrice1 = AutoFixture.Create<int>();
            var item2 = AutoFixture.Create<string>();
            var expectedPrice2 = AutoFixture.Create<int>();

            var calculator1 = new Mock<ISkuPriceCalculator>();
            calculator1.Setup(calc => calc.TotalPrice()).Returns(expectedPrice1);
            var calculator2 = new Mock<ISkuPriceCalculator>();
            calculator2.Setup(calc => calc.TotalPrice()).Returns(expectedPrice2);
            Mocker.GetMock<ISkuPriceCalculatorFactory>()
                .Setup(factory => factory.Build(item1))
                .Returns(calculator1.Object);
            Mocker.GetMock<ISkuPriceCalculatorFactory>()
                .Setup(factory => factory.Build(item2))
                .Returns(calculator2.Object);

            // Act
            subject.Scan(item1);
            subject.Scan(item2);
            var result = subject.GetTotalPrice();

            // Assert
            result.Should().Be(expectedPrice1 + expectedPrice2);
        }

        [Fact]
        public void WhenScanningNUniqueItemsThenThePriceIsTheExpectedPrice()
        {
            // Arrange
            ICheckout subject = Mocker.CreateInstance<Checkout>();

            var itemCount = Random.Next(1, 11);
            var itemList = new List<string>();
            var totalExpectedPrice = 0;
            for(var i = 0; i < itemCount; i++)
            {
                var item = AutoFixture.Create<string>();
                itemList.Add(item);
                var expectedPrice = AutoFixture.Create<int>();
                totalExpectedPrice += expectedPrice;

                var calculator = new Mock<ISkuPriceCalculator>();
                calculator.Setup(calc => calc.TotalPrice()).Returns(expectedPrice);
                Mocker.GetMock<ISkuPriceCalculatorFactory>()
                    .Setup(factory => factory.Build(item))
                    .Returns(calculator.Object);
            }

            // Act
            foreach (var item in itemList)
            {
                subject.Scan(item);
            }
            var result = subject.GetTotalPrice();

            // Assert
            result.Should().Be(totalExpectedPrice);
        }

        [Fact]
        public void WhenAnItemIsScannedTwiceThenThePriceIsTheExpectedPrice()
        {
            // Arrange
            ICheckout subject = Mocker.CreateInstance<Checkout>();

            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();

            var calculator = Mocker.GetMock<ISkuPriceCalculator>();
            calculator.Setup(calc => calc.IsCalculatingPriceForItem(item)).Returns(true);
            calculator.Setup(calc => calc.TotalPrice()).Returns(expectedPrice);
            Mocker.GetMock<ISkuPriceCalculatorFactory>()
                .Setup(factory => factory.Build(item))
                .Returns(calculator.Object);

            // Act
            subject.Scan(item);
            subject.Scan(item);
            var result = subject.GetTotalPrice();

            // Assert
            result.Should().Be(expectedPrice);
            calculator.Verify(calc => calc.IncrementItemCount(), Times.Once());
        }

        [Fact]
        public void WhenAnItemIsScannedNTimesThenThePriceIsTheExpectedPrice()
        {
            // Arrange
            ICheckout subject = Mocker.CreateInstance<Checkout>();

            var item = AutoFixture.Create<string>();
            var expectedPrice = AutoFixture.Create<int>();
            var numberOfScans = Random.Next(1, 11);

            var calculator = Mocker.GetMock<ISkuPriceCalculator>();
            calculator.Setup(calc => calc.IsCalculatingPriceForItem(item)).Returns(true);
            calculator.Setup(calc => calc.TotalPrice()).Returns(expectedPrice);
            Mocker.GetMock<ISkuPriceCalculatorFactory>()
                .Setup(factory => factory.Build(item))
                .Returns(calculator.Object);

            // Act
            var i = 0;
            while (i++ < numberOfScans)
            {
                subject.Scan(item);
            }
            var result = subject.GetTotalPrice();

            // Assert
            result.Should().Be(expectedPrice);
            calculator.Verify(calc => calc.IncrementItemCount(), Times.Exactly(numberOfScans - 1));
        }

        internal class ItemTestDetails
        {
            public ItemTestDetails(string item, int expectedPrice, int scanCount, Mock<ISkuPriceCalculator> skuPriceCalculatorMock)
            {
                Item = item;
                ExpectedPrice = expectedPrice;
                ScanCount = scanCount;
                SkuPriceCalculatorMock = skuPriceCalculatorMock;
            }

            public string Item { get; private set; }
            public int ExpectedPrice { get; private set; }
            public int ScanCount { get; private set; }
            public Mock<ISkuPriceCalculator> SkuPriceCalculatorMock { get; private set; }
        }

        [Fact]
        public void WhenScanningNUniqueItemsUpToMTimesThenThePriceIsTheExpectedPrice()
        {
            // Arrange
            ICheckout subject = Mocker.CreateInstance<Checkout>();

            var itemCount = Random.Next(1, 11);
            var itemList = new List<ItemTestDetails>();
            for (var i = 0; i < itemCount; i++)
            {
                var testDetails = new ItemTestDetails(AutoFixture.Create<string>(), AutoFixture.Create<int>(),
                    Random.Next(1, 11), new Mock<ISkuPriceCalculator>());

                testDetails.SkuPriceCalculatorMock.Setup(calc => calc.TotalPrice()).Returns(testDetails.ExpectedPrice);
                testDetails.SkuPriceCalculatorMock.Setup(calc => calc.IsCalculatingPriceForItem(testDetails.Item))
                    .Returns(true);
                Mocker.GetMock<ISkuPriceCalculatorFactory>()
                    .Setup(factory => factory.Build(testDetails.Item))
                    .Returns(testDetails.SkuPriceCalculatorMock.Object);
                itemList.Add(testDetails);
            }

            var scanList = new List<string>();
            foreach (var itemTestDetails in itemList)
            {
                for (int i = 0; i < itemTestDetails.ScanCount; i++)
                {
                    scanList.Add(itemTestDetails.Item);
                }
            }

            // Act
            foreach (var item in scanList)
            {
                subject.Scan(item);
            }
            var result = subject.GetTotalPrice();

            // Assert
            result.Should().Be(itemList.Sum(itd => itd.ExpectedPrice));
            foreach (var itemTestDetails in itemList)
            {
                itemTestDetails.SkuPriceCalculatorMock.Verify(calc => calc.IncrementItemCount(), Times.Exactly(itemTestDetails.ScanCount - 1));
            }
        }
    }

    public interface ISkuPriceCalculatorFactory
    {
        ISkuPriceCalculator Build(string item);
    }

    public interface ISkuPriceCalculator
    {
        int TotalPrice();
        void IncrementItemCount();
        bool IsCalculatingPriceForItem(string item);
    }

    public interface ICheckout
    {
        void Scan(string item);
        int GetTotalPrice();
    }

    public class Checkout : ICheckout
    {
        private readonly ISkuPriceCalculatorFactory _skuPriceCalculatorFactory;
        private readonly List<ISkuPriceCalculator> _skuPriceCalculatorList;

        public Checkout(ISkuPriceCalculatorFactory skuPriceCalculatorFactory)
        {
            _skuPriceCalculatorFactory = skuPriceCalculatorFactory;
            _skuPriceCalculatorList = new List<ISkuPriceCalculator>();
        }

        public void Scan(string item)
        {
            var calculator = _skuPriceCalculatorList.Find(calc => calc.IsCalculatingPriceForItem(item));

            if (calculator == null)
            {
                _skuPriceCalculatorList.Add(_skuPriceCalculatorFactory.Build(item));
            }
            else
            {
                calculator.IncrementItemCount();
            }
        }

        public int GetTotalPrice()
        {
            return _skuPriceCalculatorList.Sum(calculator => calculator.TotalPrice());
        }
    }
}