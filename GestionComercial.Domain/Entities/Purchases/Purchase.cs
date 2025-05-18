using GestionComercial.Domain.Constant;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Purchases
{
    public class Purchase : CommonEntity
    {

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Proveedor")]
        public int ProviderId { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Número de Comprobante")]
        [Range(1, 99999999, ErrorMessage = "Debe seleccionar el {0}")]
        public long BillNumber { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Número de Punto Venta")]
        [Range(1, 99999, ErrorMessage = "Debe seleccionar el {0}")]
        public int BillPoint { get; set; }


        public string BillNumberString => string.Format("{0:00000}-{1:00000000}", BillPoint, BillNumber);

        public string BillNumberStringWithVaucherType => string.Format("{0} {1:00000}-{2:00000000}", EnumExtensionService.GetDisplayName(VaucherType), BillPoint, BillNumber);


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Forma de pago")]
        [Range(1, 1000, ErrorMessage = "Debe seleccionar el {0}")]
        public SaleCondition SaleCondition { get; set; }


        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Tipo Comprobante")]
        //[Index("BillNumber_Provider_Index", 4, IsUnique = true)]
        [Range(1, 999, ErrorMessage = "Debe seleccionar el {0}")]
        public VaucherType VaucherType { get; set; }



        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de compra")]
        public DateTime PurchaseDate { get; set; }



        [Display(Name = "Importe total de la factura")]
        public decimal Total { get; set; }


        public decimal SubTotal { get; set; }


        public decimal GeneralDiscount { get; set; }


        public decimal InternalTax { get; set; }


        public decimal TotalIVA21 { get; set; }


        public decimal TotalIVA105 { get; set; }


        public decimal TotalIVA27 { get; set; }



        public bool PaidOut { get; set; }



        public bool IsFinished { get; set; }



        public bool IsCancelable { get; set; }







        [JsonIgnore] 
        public virtual Provider? Provider { get; set; }

        [JsonIgnore] 
        public virtual ICollection<PurchaseDetail>? PurcheaseDetails { get; set; }

        [JsonIgnore] 
        public virtual ICollection<PurchaseDetailTmp>? PurchaseDetailTmps { get; set; }
    }
}
