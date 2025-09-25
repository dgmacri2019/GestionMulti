namespace GestionComercial.Domain.DTOs.Security
{
    public class SecurityFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public string? UserId { get; set; }
    }
}
