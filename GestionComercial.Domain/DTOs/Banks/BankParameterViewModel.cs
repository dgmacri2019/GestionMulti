using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.DTOs.Banks
{
    public class BankParameterViewModel
    {
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Creado el")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Creado por")]
        public string CreateUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modificado el")]
        public DateTime? UpdateDate { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Modificado por")]
        public string? UpdateUser { get; set; }

        [Display(Name = "Borrado?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Habilitado?")]
        public bool IsEnabled { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Banco")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int BankId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condicion Venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int SaleConditionId { get; set; }

        public string SaleConditionString { get; set; }


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

        public string BankName { get; set; }



        public ICollection<Bank> Banks { get; set; }

        public ICollection<SaleCondition> SaleConditions { get; set; }
       
    }
}
