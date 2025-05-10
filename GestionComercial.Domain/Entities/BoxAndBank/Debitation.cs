using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Purchases;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.BoxAndBank
{
    public class Debitation : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Banco")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        public int BankId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Venta")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int PurchaseId { get; set; }

        [Display(Name = "Día de díbito")]
        public DateTime DebitationDate { get; set; }

        [Display(Name = "Debitado")]
        public bool IsDebited { get; set; }

        [Display(Name = "Importa a debitar")]
        public decimal FromDebit { get; set; }

        [Display(Name = "Importe Total Compra")]
        public decimal TotalPurchase { get; set; }

        public int SaleId { get; set; }



        public virtual Bank Bank { get; set; }

        public virtual Purchase Purchase { get; set; }


    }
}
