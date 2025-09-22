using System.ComponentModel;

namespace GestionComercial.Domain.DTOs.User
{
    public class UserViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //TODO: Ver Properties Change
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? RoleName { get; set; }
        public string FirstName { get; set; }
        public string? Password { get; set; }
        public string LastName { get; set; }
        public bool ChangePassword { get; set; }
        public int RoleId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }



        public List<UserRoleDto> UserRoleDtos { get; set; } = [];
    }


    public class UserRoleDto
    {
        public required string Name { get; set; }
        public required int Id { get; set; }
    }
}
