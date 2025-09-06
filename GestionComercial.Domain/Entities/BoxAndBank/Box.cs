using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.BoxAndBank
{
    public class Box : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(300, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [Display(Name = "Nombre")]
        public required string BoxName { get; set; }

        [Display(Name = "Por Debitar")]
        public decimal FromDebit { get; set; }

        [Display(Name = "Por Acreditar")]
        public decimal FromCredit { get; set; }

        [Display(Name = "Saldo")]
        public decimal Sold { get; set; }

        public int SaleConditionId { get; set; }

        [Display(Name = "Cuanta Contable")]
        public int AccountId { get; set; }



        [JsonIgnore]
        public virtual Account? Account { get; set; }

        [JsonIgnore]
        public virtual SaleCondition? SaleCondition { get; set; }


    }
}
