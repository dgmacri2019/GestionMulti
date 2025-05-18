using GestionComercial.Domain.Entities.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class PurchaseOrder : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Proveedor")]
        public int ProviderId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de orden de compra")]
        public DateTime PurchaseOrderDate { get; set; }

        [Display(Name = "Importe total de la factura")]
        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GeneralDiscount { get; set; }

        public decimal InternalTax { get; set; }

        public decimal TotalIVA21 { get; set; }

        public decimal TotalIVA105 { get; set; }

        public decimal TotalIVA27 { get; set; }

        public bool IsFinished { get; set; }

        public bool IsCancelable { get; set; }

        [Display(Name = "Número orden de compra")]
        [Range(1, 99999999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int PurchaseOrderNumber { get; set; }

        [Display(Name = "Sucursal orden de compra")]
        [Range(1, 99999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int PurchaseOrderPoint { get; set; }

        public string PurchaseOrderNumberString => string.Format("{0:00000}-{1:00000000}", PurchaseOrderPoint, PurchaseOrderNumber);



        [JsonIgnore] 
        public virtual ICollection<PurchaseOrderDetail>? PurchaseOrderDetails { get; set; }

        [JsonIgnore] 
        public virtual Provider? Provider { get; set; }

    }
}
