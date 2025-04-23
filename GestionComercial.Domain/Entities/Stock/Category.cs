using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Stock
{
    public class Category : CommonEntity
    {
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Description { get; set; }


        [Display(Name = "Color")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string? Color { get; set; }




        public virtual ICollection<Article>? Articles { get; set; }
    }
}
