using GestionComercial.Domain.Entities.Masters;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchasePayMetodDetail : CommonEntity
    {
        public int PurchaseId { get; set; }

        public SaleCondition SaleCondition { get; set; }

        public decimal Value { get; set; }



        [JsonIgnore] 
        public virtual Purchase? Purchase { get; set; }
    }
}
