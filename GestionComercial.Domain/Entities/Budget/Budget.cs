using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Budget
{
    public class Budget : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }

        [Display(Name = "Venta Nro")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de Presupuesto")]
        public DateTime BudgetDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Forma de pago")]
        public SaleCondition SaleCondition { get; set; }

        [Display(Name = "Importe total de la venta")]
        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GeneralDiscount { get; set; }

        public decimal InternalTax { get; set; }

        public decimal TotalIVA21 { get; set; }

        public decimal TotalIVA105 { get; set; }

        public decimal TotalIVA27 { get; set; }

        public decimal BaseImp21 { get; set; }

        public decimal BaseImp105 { get; set; }

        public decimal BaseImp27 { get; set; }

        public bool IsFinished { get; set; }

        public int BudgetNumber { get; set; }

        public int BudgetPoint { get; set; }

        [Display(Name = "Presupuesto Válido Hasta")]
        public DateTime ValidTo { get; set; }

        public string BudgetNumberString => string.Format("{0:00000}-{1:00000000}", BudgetPoint, BudgetNumber);




        [JsonIgnore] 
        public virtual Client? Client { get; set; }


        [JsonIgnore] 
        public virtual ICollection<BudgetDetail>? BudgetDetails { get; set; }

    }
}
