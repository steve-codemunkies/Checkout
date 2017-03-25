namespace Checkout.Interfaces
{
    public interface ISkuPriceCalculator
    {
        int TotalPrice();
        void IncrementItemCount();
        bool IsCalculatingPriceForItem(string item);
    }
}