using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.DTOs.Stock
{
    public class ArticleViewModel
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

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
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = false)]
        //[Range(0, double.MaxValue, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Bonificación / Recargo")]
        [Range(-100, 100, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal Bonification { get; set; }

        [Display(Name = "Precio de compra")]
        [DisplayFormat(DataFormatString = "{0:C4}", ApplyFormatInEditMode = false)]
        //[Range(0, double.MaxValue, ErrorMessage = "Debe seleccion un {0} entre {1} y {2}")]
        public decimal RealCost { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar un {0}")]
        [Display(Name = "Tipo de IVA")]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es requerido")]
        [Display(Name = "Impuestos Internos")]
        [DisplayFormat(DataFormatString = "{0:P0}", ApplyFormatInEditMode = false)]
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
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal Stock { get; set; }

        [Display(Name = "Stock Mínimo")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal MinimalStock { get; set; }

        [Display(Name = "Reposición")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
        public decimal Replacement { get; set; }

        [Display(Name = "Umbral")]
        [DisplayFormat(DataFormatString = "{0:N4}", ApplyFormatInEditMode = false)]
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

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Creado el")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Creado por")]
        public string CreateUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modificado el")]
        public DateTime? UpdateDate { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Modificado por")]
        public string? UpdateUser { get; set; }

        [Display(Name = "Borrado?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Habilitado?")]
        public bool IsEnabled { get; set; }

        
        public List<PriceListItemDto> PriceLists { get; set; } = [];
        public List<TaxePriceDto> TaxesPrice { get; set; } = [];

        public string Category { get; set; } = string.Empty;
        public string CategoryColor { get; set; } = string.Empty;
        public decimal PriceWithTax { get; set; }


        public virtual ICollection<Tax> Taxes { get; set; }
        public virtual ICollection<Measure> Measures { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }


    public class PriceListItemDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal FinalPrice { get; set; }
    }

    public class TaxePriceDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Utility { get; set; }
        public decimal Price { get; set; }
    }

}
