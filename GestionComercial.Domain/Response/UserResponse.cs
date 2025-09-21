using GestionComercial.Domain.DTOs.User;

namespace GestionComercial.Domain.Response
{
    public class UserResponse : GeneralResponse
    {
        public List<UserViewModel>? UserViewModels { get; set; }
        public UserViewModel? UserViewModel { get; set; }
        public int TotalRegisters { get; set; }
    }
}
