using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Masters.Security
{
    public class Permission : CommonEntity
    {
        public Permission()
        {
            RolePermissions = [];
            UserPermissions = [];
        }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Módulo")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public ModuleType ModuleType { get; set; }




        // Relación con RolePermission (muchos a muchos con Roles)
        [JsonIgnore] 
        public virtual ICollection<RolePermission> RolePermissions { get; set; }

        // Relación con UserPermission (para permisos asignados directamente a un usuario)
        [JsonIgnore] 
        public virtual ICollection<UserPermission> UserPermissions { get; set; }
    }
}