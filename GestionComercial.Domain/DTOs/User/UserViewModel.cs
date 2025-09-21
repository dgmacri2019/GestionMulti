using System.ComponentModel;

namespace GestionComercial.Domain.DTOs.User
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //TODO: Ver Properties Change
        public required string Id { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
        public required string FirstName { get; set; }
        public string? Password { get; set; }
        public required string LastName { get; set; }
        public bool ChangePassword { get; set; }
        public bool Enabled { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }


    }
}
