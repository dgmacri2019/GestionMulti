using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Masters
{
    public class State : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Provincia")]
        [MaxLength(40, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        public string Name { get; set; }


        [Display(Name = "Id Afip")]
        public int AfipId { get; set; }




        public virtual ICollection<User>? Users { get; set; }

        public virtual ICollection<Provider>? Providers { get; set; }
    }
}
