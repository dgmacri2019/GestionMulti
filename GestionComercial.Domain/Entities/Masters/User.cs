using GestionComercial.Domain.Entities.Masters.Security;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Nombres")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Apellidos")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cambiar Password?")]
        public bool ChangePassword { get; set; }

        [Display(Name = "Habilitado?")]
        public bool Enabled { get; set; }






        //[Required(ErrorMessage = "El campo {0} es requerido")]
        //[Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        //public int RoleId { get; set; }

        public string FullName => string.Format("{0}, {1}", FirstName, LastName);



        public List<string> Roles { get; set; } = [];


        //[JsonIgnore] 
        public ICollection<UserPermission>? UserPermissions { get; set; }
    }

}

