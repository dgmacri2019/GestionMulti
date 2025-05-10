using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.AccountingBook
{
    public class AccountType : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Es activo")]
        public bool IsActive { get; set; }

        [Display(Name = "Es pasivo")]
        public bool IsPasive { get; set; }

        [Display(Name = "Es Patrimonio Neto")]
        public bool IsNetHeritage { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
