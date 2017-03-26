namespace Checkout.Exception
{
    public class SkuNotFoundException : System.Exception
    {
        public SkuNotFoundException(string item)
        {
            Item = item;
        }

        public string Item { get; private set; }
    }
}