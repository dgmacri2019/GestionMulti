using GestionComercial.Domain.Entities.Masters.Security;

namespace GestionComercial.Domain.Response
{
    public class LoginResponse : GeneralResponse
    {
        public string? Token { get; set; }
        public List<Permission> Permissions { get; set; }
        public string UserId { get; set; }
    }
}
