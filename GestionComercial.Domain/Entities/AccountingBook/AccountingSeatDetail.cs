using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.AccountingBook
{
    public class AccountingSeatDetail : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int AccountingSeatId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cuenta al debe")]
        [Range(1, int.MaxValue, ErrorMessage = "debe seleccionar una {0}")]
        public int OriginAccountId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Tipo de cuenta de Origen")]
        [Range(1, int.MaxValue, ErrorMessage = "debe seleccionar una {0}")]
        public int OriginAccountTypeId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cuenta al haber")]
        [Range(1, int.MaxValue, ErrorMessage = "debe seleccionar una {0}")]
        public int DestinationAccountId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Tipo de cuenta de Destino")]
        [Range(1, int.MaxValue, ErrorMessage = "debe seleccionar una {0}")]
        public int DestinationAccountTypeId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Importe")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar el {0}")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Value { get; set; }


        [Display(Name = "Al Debe")]
        public bool ToOwe { get; set; }


        [Display(Name = "Al Haber")]
        public bool ToHave { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha del movimiento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }


        [Display(Name = "Es Conciliación")]
        public bool IsConciliate { get; set; }



        public virtual AccountingSeat AccountingSeat { get; set; }
    }
}
