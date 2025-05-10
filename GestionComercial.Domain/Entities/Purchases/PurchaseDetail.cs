using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchaseDetail : CommonEntity
    {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Price { get; set; }

        public decimal RealCost { get; set; }

        [Display(Name = "Descuento")]
        public int Discount { get; set; }

        public decimal SubTotal { get; set; }

        public decimal TotalItem { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int TaxId { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int PurcheaseId { get; set; }




        public virtual Article Article { get; set; }

        public virtual Tax Tax { get; set; }

        public virtual Purchase Purchease { get; set; }

    }
}
