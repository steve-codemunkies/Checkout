using System;
using Checkout.Interfaces;

namespace Checkout
{
    public class SkuPriceCalculator : ISkuPriceCalculator
    {
        private int _count = 1;
        private readonly string _item;
        private readonly int _unitPrice;
        private readonly int? _multiBuyItemCount;
        private readonly int? _multiBuyPrice;

        public SkuPriceCalculator(string item, int unitPrice, int? multiBuyItemCount = null, int? multiBuyPrice = null)
        {
            _item = item;
            _unitPrice = unitPrice;
            _multiBuyItemCount = multiBuyItemCount;
            _multiBuyPrice = multiBuyPrice;
        }

        public int TotalPrice()
        {
            if (_multiBuyItemCount.HasValue && _multiBuyPrice.HasValue)
            {
                return ((_count / _multiBuyItemCount.Value) * _multiBuyPrice.Value) + ((_count % _multiBuyItemCount.Value) * _unitPrice);
            }

            return _unitPrice * _count;
        }

        public void IncrementItemCount()
        {
            _count++;
        }

        public bool IsCalculatingPriceForItem(string item)
        {
            return string.Compare(_item, item, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}