using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.BoxAndBank
{
    public class Acreditation : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Banco")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int BankId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int SaleId { get; set; }


        [Display(Name = "Día de acreditación")]
        public DateTime AcreditationDate { get; set; }


        [Display(Name = "Acreditado")]
        public bool IsAcredited { get; set; }


        [Display(Name = "Importa a acreditar")]
        public decimal FromAcredit { get; set; }


        [Display(Name = "Importe Total Venta")]
        public decimal TotalSale { get; set; }



        [JsonIgnore] 
        public virtual Bank? Bank { get; set; }

        [JsonIgnore] 
        public virtual Sale? Sale { get; set; }

    }
}
