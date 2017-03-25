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

        public CheckoutTests()
        {
            AutoFixture = new Fixture();
            Mocker = new AutoMocker();
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
    }

    public interface ISkuPriceCalculatorFactory
    {
        ISkuPriceCalculator Build(string item);
    }

    public interface ISkuPriceCalculator
    {
        int TotalPrice();
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
            _skuPriceCalculatorList.Add(_skuPriceCalculatorFactory.Build(item));
        }

        public int GetTotalPrice()
        {
            return _skuPriceCalculatorList.Sum(calculator => calculator.TotalPrice());
        }
    }
}