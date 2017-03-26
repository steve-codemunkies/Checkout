using Checkout.Data.Dto;
using NHibernate;

namespace Checkout.Data
{
    public class GetSkuWithMultiBuy : IGetSkuWithMultiBuy
    {
        private readonly ISession _session;

        public GetSkuWithMultiBuy(ISession session)
        {
            _session = session;
        }

        public SkuWithMultiBuy Query(string item)
        {
            return
                _session.QueryOver<SkuWithMultiBuy>().WhereRestrictionOn(dto => dto.Item).IsLike(item).SingleOrDefault();
        }
    }
}