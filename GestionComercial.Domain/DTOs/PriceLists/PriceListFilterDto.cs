namespace GestionComercial.Domain.DTOs.PriceLists
{
    public class PriceListFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
    }
}
