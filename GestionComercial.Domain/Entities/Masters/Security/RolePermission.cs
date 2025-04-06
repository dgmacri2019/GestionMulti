using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class RolePermission : CommonEntity
    {
        // Clave foránea de Role (suponiendo que uses ApplicationRole de Identity)
        public string RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual IdentityRole Role { get; set; }


        // Clave foránea de Permission
        public int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
    }
}
