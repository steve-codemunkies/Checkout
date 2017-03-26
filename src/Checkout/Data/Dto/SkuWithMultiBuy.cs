using FluentNHibernate.Mapping;

namespace Checkout.Data.Dto
{
    public class SkuWithMultiBuy
    {
        public virtual int Id { get; set; }
        public virtual string Item { get; set; }
        public virtual int UnitPrice { get; set; }
        public virtual int? MultiBuyItemCount { get; set; }
        public virtual int? MultiBuyPrice { get; set; }

        public class SkuWithMultiBuyMap : ClassMap<SkuWithMultiBuy>
        {
            public SkuWithMultiBuyMap()
            {
                Schema("dbo");
                Table("SkuWithMultiBuy");
                Id(dto => dto.Id);
                Map(dto => dto.Item);
                Map(dto => dto.UnitPrice);
                Map(dto => dto.MultiBuyItemCount);
                Map(dto => dto.MultiBuyPrice);
            }
        }
    }
}