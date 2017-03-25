using FluentAssertions;
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

            var calculator =  Mocker.GetMock<ISkuPriceCalculator>();
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
        private ISkuPriceCalculator _skuPriceCalculator;

        public Checkout(ISkuPriceCalculatorFactory skuPriceCalculatorFactory)
        {
            _skuPriceCalculatorFactory = skuPriceCalculatorFactory;
        }

        public void Scan(string item)
        {
            _skuPriceCalculator = _skuPriceCalculatorFactory.Build(item);
        }

        public int GetTotalPrice()
        {
            return _skuPriceCalculator.TotalPrice();
        }
    }
}