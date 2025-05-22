namespace GestionComercial.Domain.DTOs.Accounts
{
    public class AccountFilterDto
    {
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool All { get; set; } = false;
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
    }
}
