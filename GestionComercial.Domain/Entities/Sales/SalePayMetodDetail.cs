using GestionComercial.Domain.Entities.Masters;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Sales
{
    public class SalePayMetodDetail : CommonEntity
    {
        public int SaleId { get; set; }

        public SaleCondition SaleCondition { get; set; }

        public decimal Value { get; set; }



        [JsonIgnore] 
        public virtual Sale? Sale { get; set; }
    }
}
