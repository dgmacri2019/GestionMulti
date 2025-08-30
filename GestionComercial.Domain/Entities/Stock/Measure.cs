using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Stock
{
    public class Measure : CommonEntity
    {
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Description { get; set; }

        [Display(Name = "Descripción Corta")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(10, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string SmallDescription { get; set; }




        [JsonIgnore] 
        public virtual ICollection<Article>? Articles { get; set; }
    }
}
