namespace GestionComercial.Domain.DTOs.Stock
{
    public class ArticleWithPricesDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? BarCode { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public List<PriceListItemDto> PriceLists { get; set; } = new();
        public string Category { get; set; } = string.Empty;
    }
}
