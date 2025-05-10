using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.AccountingBook
{
    public class AccountingSeat : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Asiento Nro.")]
        [Range(1, int.MaxValue, ErrorMessage = "debe seleccionar una {0}")]
        public int AccountingSeatNumber { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Asiento Nro.")]
        [Range(1, int.MaxValue, ErrorMessage = "debe seleccionar una {0}")]
        public int AccountingSeatYear { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Concepto")]
        [MaxLength(350, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        public string Concept { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Documento respaldatorio")]
        [MaxLength(350, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        public string DocumentDescription { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha del movimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Importe")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }


        [Display(Name = "Asiento Nro.")]
        public string AccountingSeatNumberString => string.Format("{0}/{1}", AccountingSeatNumber, Date.Year);


        public virtual ICollection<AccountingSeatDetail> AccountingSeatDetails { get; set; }
    }
}
