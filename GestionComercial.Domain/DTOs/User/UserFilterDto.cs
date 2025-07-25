namespace GestionComercial.Domain.DTOs.User
{
    public class UserFilterDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsEnabled { get; set; }
        public bool All { get; set; }
        public string? PhoneNumber { get; set; }
        public string? NameFilter { get; set; }


    }
}
