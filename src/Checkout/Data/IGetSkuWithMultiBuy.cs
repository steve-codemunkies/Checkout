using Checkout.Data.Dto;

namespace Checkout.Data
{
    public interface IGetSkuWithMultiBuy
    {
        SkuWithMultiBuy Query(string item);
    }
}