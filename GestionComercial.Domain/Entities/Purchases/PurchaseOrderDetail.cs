using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchaseOrderDetail : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int ArticleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int PurchaseOrderId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Quantity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public decimal Price { get; set; }

        public decimal TotalItem { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int TaxId { get; set; }



        [JsonIgnore] 
        public virtual Article? Article { get; set; }

        [JsonIgnore]
        public virtual PurchaseOrder? PurcheaseOrder { get; set; }

        [JsonIgnore]
        public virtual Tax? Tax { get; set; }
    }
}
