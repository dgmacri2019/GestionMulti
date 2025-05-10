using GestionComercial.Domain.Entities.Masters;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchasePayMetodDetail : CommonEntity
    {
        public int PurchaseId { get; set; }

        public SaleCondition SaleCondition { get; set; }

        public decimal Value { get; set; }



        public virtual Purchase Purchase { get; set; }
    }
}
