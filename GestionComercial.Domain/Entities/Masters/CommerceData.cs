using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Masters
{
    public class CommerceData : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Versión del Sistema")]
        public SystemVersionType SystemVersionType { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Razón Social")]
        public string BusinessName { get; set; }

        [Display(Name = "Nombre de Fantasía")]
        public string? FantasyName { get; set; }

        [Required]
        [Display(Name = "Número de CUIT")]
        public long CUIT { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Condición de IVA")]
        [Range(1, double.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public TaxCondition TaxCondition { get; set; }

        [Display(Name = "Ingresos Brutos")]
        public string IIBB { get; set; }

        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Código Postal")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Provincia")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
        public int CityId { get; set; }

        [Display(Name = "Inicio de actividades")]
        public DateTime? ActivityStartDate { get; set; }

        [Display(Name = "Correo Electrónico")]
        public string? Email { get; set; }

        [Display(Name = "Teléfono / Fax")]
        public string? Phone { get; set; }

        [Display(Name = "Teléfono / Fax")]
        public string? Phone2 { get; set; }

        [Display(Name = "Teléfono / Fax")]
        public string? Phone3 { get; set; }

        [Display(Name = "Pagina Web")]
        public string? WebSite { get; set; }

        [Display(Name = "CBU Bancario")]
        public string? CBU { get; set; }

        [Display(Name = "Alias CBU Bancario")]
        public string? Alias { get; set; }

        [Required]
        [Display(Name = "Código de Activación")]
        public string? ActivationCode { get; set; }

        [Required]
        [Display(Name = "Email de registro")]
        public string? RegisterEmail { get; set; }

        [Display(Name = "Servicio válido hasta")]
        public DateTime ServiceValidTo { get; set; }

        [Display(Name = "Servicio Habilitado")]
        public bool ServiceEnable { get; set; }

        [Display(Name = "Utilizar Cuantas Contables")]
        public bool UseAccounting { get; set; }




        [JsonIgnore] 
        public virtual State? State { get; set; }

        [JsonIgnore] 
        public virtual City? City { get; set; }

        [JsonIgnore] 
        public ICollection<Billing>? Billings { get; set; }

    }
}
