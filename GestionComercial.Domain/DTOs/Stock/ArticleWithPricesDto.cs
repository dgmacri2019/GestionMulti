namespace GestionComercial.Domain.DTOs.Stock
{
    public class ArticleWithPricesDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? BarCode { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Bonification { get; set; }
        public decimal RealCost { get; set; }
        public List<PriceListItemDto> PriceLists { get; set; } = new();
        public List<TaxePriceDto> TaxesPrice { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
        public decimal PriceWithTax { get; set; }
        public decimal Stock { get; set; }
    }

    public class PriceListItemDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class TaxePriceDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal Price { get; set; }
    }

   
}
