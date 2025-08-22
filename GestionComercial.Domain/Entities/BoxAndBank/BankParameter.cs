using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.BoxAndBank
{
    public class BankParameter : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Banco")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int BankId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condicion Venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int SaleConditionId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Comisión")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public decimal Rate { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Días para Acreditación")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int AcreditationDay { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Días para Débito")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int DebitationDay { get; set; }



        [JsonIgnore] 
        public virtual Bank? Bank { get; set; }

        [JsonIgnore] 
        public virtual SaleCondition? SaleCondition { get; set; }

    }
}
