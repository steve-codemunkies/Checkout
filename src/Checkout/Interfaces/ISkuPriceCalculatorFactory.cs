namespace Checkout.Interfaces
{
    public interface ISkuPriceCalculatorFactory
    {
        ISkuPriceCalculator Build(string item);
    }
}