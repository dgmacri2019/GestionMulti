using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class RolePermission : CommonEntity
    {
        // Clave foránea de Role (suponiendo que uses ApplicationRole de Identity)
        public string RoleId { get; set; }

        // Clave foránea de Permission
        public int PermissionId { get; set; }



        //[JsonIgnore] 
        public virtual IdentityRole? Role { get; set; }

        //[JsonIgnore] 
        public virtual Permission? Permission { get; set; }
    }
}
