using GestionComercial.Domain.Entities.Purchases;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Masters
{
    public class Provider : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(300, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Razón Social")]
        public string BusinessName { get; set; }

        [MaxLength(300, ErrorMessage = "El campo {0} debe contener un máximo de {1} caracteres")]
        [Display(Name = "Nombre de Fantasía")]
        public string? FantasyName { get; set; }

        [Display(Name = "Tipo Documento")]
        public DocumentType DocumentType { get; set; }

        [MaxLength(13, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [MinLength(7, ErrorMessage = "El {0} no puede contener menos de {1} caracteres")]
        [Display(Name = "Número Documento")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone1 { get; set; }

        [Display(Name = "Teléfono / Fax")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Phone2 { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Dirección")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Código Postal")]
        [MaxLength(8, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string PostalCode { get; set; }

        [Display(Name = "E-Mail")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Provincia")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Localidad")]
        [MaxLength(500, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? City { get; set; }

        [Display(Name = "Sitio Web")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? WebSite { get; set; }

        [Display(Name = "Sitio Web")]
        [DataType(DataType.MultilineText)]
        public string? Remark { get; set; }

        [Display(Name = "Fecha de última compra")]
        public DateTime? LastPuchase { get; set; }

        [Display(Name = "Saldo Pendiente")]
        public decimal Sold { get; set; }

        [Display(Name = "Día de pago")]
        [MaxLength(200, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? PayDay { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Metodo de pago")]
        public SaleCondition SaleCondition { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condición de IVA")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public TaxCondition TaxCondition { get; set; }




        [JsonIgnore] 
        public virtual State? State { get; set; }

        [JsonIgnore] 
        public virtual ICollection<Purchase>? Purcheases { get; set; }
    }
}
