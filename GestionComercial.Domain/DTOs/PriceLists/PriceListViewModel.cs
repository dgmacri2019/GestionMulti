
namespace GestionComercial.Domain.DTOs.PriceLists
{
    public class PriceListViewModel
    {
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string Description { get; set; }
        public int Id { get; set; }
        public decimal Utility { get; set; }
    }
}
