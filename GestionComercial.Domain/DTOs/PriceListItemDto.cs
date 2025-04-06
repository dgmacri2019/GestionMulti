namespace GestionComercial.Domain.DTOs
{
    public class PriceListItemDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
