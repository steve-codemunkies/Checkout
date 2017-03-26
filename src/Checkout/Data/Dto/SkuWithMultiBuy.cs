namespace Checkout.Data.Dto
{
    public class SkuWithMultiBuy
    {
        public string Item { get; set; }
        public int UnitPrice { get; set; }
        public int? MultiBuyItemCount { get; set; }
        public int? MultiBuyPrice { get; set; }
    }
}