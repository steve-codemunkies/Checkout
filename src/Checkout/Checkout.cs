using System.Collections.Generic;
using System.Linq;
using Checkout.Interfaces;

namespace Checkout
{
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