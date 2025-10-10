namespace GestionComercial.Domain.DTOs.Sale
{
    public class SaleFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public int SalePoint { get; set; }
        public int SaleNumber { get; set; }
        public DateTime? SaleDate { get; set; }

        public int Page { get; set; } = 1;       // página por defecto
        public int PageSize { get; set; } = 100; // tamaño por defecto

        public string? UserName { get; set; }
    }
}
