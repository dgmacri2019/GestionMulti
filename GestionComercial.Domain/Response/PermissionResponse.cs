using GestionComercial.Domain.DTOs.Security;
using GestionComercial.Domain.Entities.Masters.Security;

namespace GestionComercial.Domain.Response
{
    public class PermissionResponse : GeneralResponse
    {
        public List<UserPermission> UserPermissions { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
