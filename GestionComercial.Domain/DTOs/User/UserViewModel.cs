namespace GestionComercial.Domain.DTOs.User
{
    public class UserViewModel
    {
        public required string Id { get; set; }
        public string? UserName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public bool ChangePassword { get; set; }
        public bool Enabled { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
