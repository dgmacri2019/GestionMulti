using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class Permission : CommonEntity
    {
        public Permission()
        {
            RolePermissions = new List<RolePermission>();
            UserPermissions = new List<UserPermission>();
        }

        [Required]
        public string Name { get; set; }

        // Relación con RolePermission (muchos a muchos con Roles)
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        // Relación con UserPermission (para permisos asignados directamente a un usuario)
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}