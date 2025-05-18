using GestionComercial.Domain.Constant;
using GestionComercial.Domain.Entities.AccountingBook;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.DTOs.Banks
{
    public class BoxViewModel
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

        [MaxLength(300, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string BoxName { get; set; }

        [Display(Name = "Por Debitar")]
        public decimal FromDebit { get; set; }

        [Display(Name = "Por Acreditar")]
        public decimal FromCredit { get; set; }

        [Display(Name = "Saldo")]
        public decimal Sold { get; set; }
        public SaleCondition SaleCondition { get; set; }

        [Display(Name = "Cuanta Contable")]
        public int AccountId { get; set; }


        public ICollection<Account> Accounts { get; set; }

        public ObservableCollection<SaleCondition> SaleConditions { get; set; }
    }
}
