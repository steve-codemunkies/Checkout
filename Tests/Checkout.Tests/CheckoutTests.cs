namespace Checkout.Tests
{
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
            _totalPrice = item == "A" ? 50 : item == "B" ? 30 : item == "C" ? 20 : 15;
        }

        public int GetTotalPrice()
        {
            return _totalPrice;
        }
    }
}