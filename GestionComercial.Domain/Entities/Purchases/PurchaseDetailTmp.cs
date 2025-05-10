using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchaseDetailTmp : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ArticleId { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Price { get; set; }

        [Display(Name = "Proveedor")]
        public int ProviderId { get; set; }

        [Display(Name = "Descuento")]
        public int Discount { get; set; }

        public decimal SubTotal { get; set; }

        [Display(Name = "Codigo")]
        public string Code { get; set; }

        [Display(Name = "Total Item")]
        public decimal TotalItem { get; set; }

        public decimal PriceDiscount => SubTotal * Discount / 100;


        public virtual Provider Provider { get; set; }

        public virtual Article Article { get; set; }
    }
}
