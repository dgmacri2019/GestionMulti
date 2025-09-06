using GestionComercial.Domain.Entities.Masters;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Sales
{
    public class SalePayMetodDetail : CommonEntity
    {
        public int SaleId { get; set; }

        public int SaleConditionId { get; set; }

        public decimal Value { get; set; }



        [JsonIgnore] 
        public virtual Sale? Sale { get; set; }
        [JsonIgnore]
        public virtual SaleCondition? SaleCondition { get; set; }
    }
}
