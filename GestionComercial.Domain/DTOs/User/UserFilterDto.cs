namespace GestionComercial.Domain.DTOs.User
{
    public class UserFilterDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
           
        public int Page { get; set; } = 1;       // página por defecto
        public int PageSize { get; set; } = 100; // tamaño por defecto

    }
}
