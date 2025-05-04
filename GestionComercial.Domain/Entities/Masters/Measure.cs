using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters
{
    public class Measure : CommonEntity
    {
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Description { get; set; }




        [JsonIgnore] 
        public virtual ICollection<Article>? Articles { get; set; }
    }
}
