using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchaseOrderDetailTmp : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ArticleId { get; set; }

        [Display(Name = "Codigo")]
        public string Code { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Price { get; set; }

        [Display(Name = "Total Item")]
        public decimal TotalItem { get; set; }

        [Display(Name = "Proveedor")]
        public int ProviderId { get; set; }



        [JsonIgnore] 
        public virtual Provider? Provider { get; set; }

        [JsonIgnore] 
        public virtual Article? Article { get; set; }
    }
}
