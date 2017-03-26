using Checkout.Data;
using Checkout.Exception;
using Checkout.Interfaces;

namespace Checkout
{
    public class SkuPriceCalculatorFactory : ISkuPriceCalculatorFactory
    {
        private readonly IGetSkuWithMultiBuy _getSkuWithMultiBuy;

        public SkuPriceCalculatorFactory(IGetSkuWithMultiBuy getSkuWithMultiBuy)
        {
            _getSkuWithMultiBuy = getSkuWithMultiBuy;
        }

        public ISkuPriceCalculator Build(string item)
        {
            var skuWithMultiBuy = _getSkuWithMultiBuy.Query(item);

            if (skuWithMultiBuy == null)
            {
                throw new SkuNotFoundException(item);
            }

            return new SkuPriceCalculator(skuWithMultiBuy.Item, skuWithMultiBuy.UnitPrice,
                skuWithMultiBuy.MultiBuyItemCount, skuWithMultiBuy.MultiBuyPrice);
        }
    }
}