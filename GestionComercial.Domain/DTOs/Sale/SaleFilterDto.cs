namespace GestionComercial.Domain.DTOs.Sale
{
    public class SaleFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public int SalePoint { get; set; }
        public DateTime? SaleDate { get; set; }
    }
}
