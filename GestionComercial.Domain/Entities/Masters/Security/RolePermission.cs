using Microsoft.AspNetCore.Identity;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class RolePermission : CommonEntity
    {
        // Clave foránea de Role (suponiendo que uses ApplicationRole de Identity)
        public string RoleId { get; set; }

        // Clave foránea de Permission
        public int PermissionId { get; set; }



        public virtual IdentityRole? Role { get; set; }

        public virtual Permission? Permission { get; set; }
    }
}
