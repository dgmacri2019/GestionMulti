using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Budget
{
    public class BudgetDetail : CommonEntity
    {
        public int BudgetId { get; set; }

        public int ArticleId { get; set; }

        public int TaxId { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public int List { get; set; }

        public int Discount { get; set; }

        public string Code { get; set; }

        public decimal PriceDiscount { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TotalItem { get; set; }




        [JsonIgnore] 
        public virtual Article? Article { get; set; }

        [JsonIgnore] 
        public virtual Tax? Tax { get; set; }

        [JsonIgnore] 
        public virtual Budget? Budget { get; set; }
    }
}
