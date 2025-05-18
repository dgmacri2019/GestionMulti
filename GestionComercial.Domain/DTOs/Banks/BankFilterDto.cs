namespace GestionComercial.Domain.DTOs.Banks
{
    public class BankFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public bool IsBank { get; set; } = false;
    }
}
