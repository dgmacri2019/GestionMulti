using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Masters.Configuration
{
    public class GeneralParameter : CommonEntity
    {
        [Display(Name = "Código de Barras en Productos Pesables con Precio")]
        public bool ProductBarCodePrice { get; set; }

        [Display(Name = "Código de Barras en Productos Pesables con Peso")]
        public bool ProductBarCodeWeight { get; set; }

        [Display(Name = "Dígito Identificador Pesables")]
        public string? WeightIdentificator { get; set; }

        [Display(Name = "Días de validez del presupuesto")]
        public int BudgetValidDays { get; set; }

        [Display(Name = "Utilizar lectoc de código de barras")]
        public bool UsePostMethod { get; set; }
    }
}
