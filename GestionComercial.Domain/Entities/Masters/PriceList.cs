using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Masters
{
    public class PriceList : CommonEntity
    {
        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        public string Description { get; set; }


        [Display(Name = "Utilidad")]
        public decimal Utility { get; set; }


    }
}
