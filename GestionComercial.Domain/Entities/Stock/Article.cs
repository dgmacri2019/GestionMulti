using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Entities.Stock
{
    public class Article : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(20, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(1000, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        [MaxLength(50, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Codigo de Barras")]
        public string? BarCode { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Precio de compra")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        //[Range(0, double.MaxValue, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Bonificación / Recargo")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Range(-100, 100, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int Bonification { get; set; }

        [Display(Name = "Precio de compra")]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        //[Range(0, double.MaxValue, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal RealCost { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Tipo de IVA")]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Impuestos Internos")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Range(0, 100, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int InternalTax { get; set; }

        //[Required(ErrorMessage = "El Campo {0} es requerido")]
        //[DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Puntos por venta")]
        [Range(0, 99999999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int SalePoint { get; set; }

        //[Required(ErrorMessage = "El Campo {0} es requerido")]
        //[DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        [Display(Name = "Puntos para canje")]
        [Range(0, 99999999, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public int ChangePoint { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Unidad de Medida")]
        public int MeasureId { get; set; }

        [Display(Name = "Verifica Stock")]
        public bool StockCheck { get; set; }

        [Display(Name = "Es Pesable")]
        public bool IsWeight { get; set; }

        [Display(Name = "Stock")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Stock { get; set; }

        [Display(Name = "Stock Mínimo")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal MinimalStock { get; set; }

        [Display(Name = "Reposición")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Replacement { get; set; }

        [Display(Name = "Umbral")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Umbral { get; set; }

        [Display(Name = "Aclaraciones")]
        public string? Clarifications { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Rubro")]
        public int CategoryId { get; set; }

        [Display(Name = "Observaciones")]
        [DataType(DataType.MultilineText)]
        public string? Remark { get; set; }



        public virtual Tax Tax { get; set; }

        public virtual Measure Measure { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<PriceList> PriceLists { get; set; }

        //public virtual ICollection<SaleDetail> SaleDetails { get; set; }

        //public virtual ICollection<SaleDetailTmp> SaleDetailTmps { get; set; }

        //public virtual ICollection<PurchaseDetail> PurcheaseDetails { get; set; }

        //public virtual ICollection<PurchaseDetailTmp> PurcheaseDetailTmps { get; set; }

    }
}
