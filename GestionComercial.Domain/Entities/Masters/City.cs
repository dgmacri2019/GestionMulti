using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters
{
    public class City : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Localidad")]
        [MaxLength(40, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        public string Name { get; set; }

        [Display(Name = "Provincia")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la provincia")]
        public int StateId { get; set; }


        [Display(Name = "Id Afip")]
        public int AfipId { get; set; }



        [JsonIgnore] 
        public virtual State? State { get; set; }

        [JsonIgnore] 
        public virtual ICollection<User>? Users { get; set; }

        [JsonIgnore]
        public virtual ICollection<Provider>? Providers { get; set; }
    }
}
