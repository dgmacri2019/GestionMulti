using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Sales
{
    public class SaleDetailTmp : CommonEntity
    {
        public int ClientId { get; set; }

        public int ArticleId { get; set; }

        public int TaxId { get; set; }

        public string Description { get; set; }

        public decimal Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal PriceWhithTax { get; set; }

        public decimal SubTotal { get; set; }

        public decimal SubTotalWhithTax { get; set; }

        public int List { get; set; }

        public int Discount { get; set; }

        public string Code { get; set; }

        public decimal PriceDiscount => SubTotal * Discount / 100;

        public decimal TotalItem { get; set; }



        [JsonIgnore] 
        public virtual Article? Article { get; set; }

        [JsonIgnore] 
        public virtual Tax? Tax { get; set; }

        [JsonIgnore] 
        public virtual Client? Client { get; set; }

    }
}
