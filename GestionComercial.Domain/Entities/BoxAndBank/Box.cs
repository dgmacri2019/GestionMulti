using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.BoxAndBank
{
    public class Box : CommonEntity
    {
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



        public virtual Account? Account { get; set; }
    }
}
