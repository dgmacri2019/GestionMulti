using GestionComercial.Domain.Entities.Afip;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Masters
{
    public class Client : CommonEntity
    {
        [MaxLength(300, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [Display(Name = "Razón social")]
        public string BusinessName { get; set; }

        [MaxLength(300, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [Display(Name = "Nombre Fantasía")]
        public string? FantasyName { get; set; }

        [Display(Name = "Tipo Documento")]
        public int DocumentTypeId { get; set; }

        [MaxLength(13, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [MinLength(7, ErrorMessage = "El {0} no puede contener menos de {1} caracteres")]
        [Display(Name = "Número Documento")]
        public string DocumentNumber { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condición de IVA")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int IvaConditionId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Dirección")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string Address { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Código Postal")]
        [MaxLength(8, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Provincia")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Localidad")]
        [MaxLength(500, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string City { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone1 { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone2 { get; set; }

        [Display(Name = "E-Mail")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Sitio Web")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? WebSite { get; set; }

        [Display(Name = "Comentarios")]
        [DataType(DataType.MultilineText)]
        public string? Remark { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condición de Venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int SaleConditionId { get; set; }

        [Display(Name = "Día de pago")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? PayDay { get; set; }

        [Display(Name = "Leyenda Facturas")]
        [DataType(DataType.MultilineText)]
        public string? LegendInvoices { get; set; }

        [Display(Name = "Leyenda Remitos")]
        [DataType(DataType.MultilineText)]
        public string? LegendRemit { get; set; }

        [Display(Name = "Leyenda Presupuestos")]
        [DataType(DataType.MultilineText)]
        public string? LegendBudget { get; set; }

        [Display(Name = "Leyenda Orden Compra")]
        [DataType(DataType.MultilineText)]
        public string? LegendOrder { get; set; }

        [Display(Name = "Ultima Compra")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastPuchase { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Lista de Precios")]
        [Range(0, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int PriceListId { get; set; }

        public string DisplayName => !string.IsNullOrWhiteSpace(FantasyName) ? FantasyName : BusinessName;

        [Display(Name = "Saldo")]
        public decimal Sold { get; set; }





        //public virtual City City { get; set; }

        [JsonIgnore]
        public virtual PriceList? PriceList { get; set; }

        [JsonIgnore]
        public virtual State? State { get; set; }

        [JsonIgnore]
        public virtual DocumentType? DocumentType { get; set; }

        [JsonIgnore]
        public virtual IvaCondition? IvaCondition { get; set; }

        [JsonIgnore]
        public virtual SaleCondition? SaleCondition { get; set; }





        //public virtual ICollection<Sale> Sales { get; set; }

        //public virtual ICollection<SaleDetailTmp> SaleDetailTmps { get; set; }
    }
}
