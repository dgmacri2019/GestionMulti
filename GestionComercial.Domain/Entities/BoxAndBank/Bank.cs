using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.BoxAndBank
{
    public class Bank : CommonEntity
    {
        [MaxLength(300, ErrorMessage = "El {0} no puede contener mas de {1} caracteres")]
        [Display(Name = "Nombre")]
        public string BankName { get; set; }

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
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar la {0}")]
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

        [Display(Name = "E-Mail")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Display(Name = "Sitio Web")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string? WebSite { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Número de cuenta")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "CBU")]
        [MaxLength(22, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        [MinLength(22, ErrorMessage = "El campo {0} no puede contener menos de {1} caracteres")]
        public string CBU { get; set; }

        [Display(Name = "Alias")]
        public string? Alias { get; set; }

        [Display(Name = "Por Debitar")]
        public decimal FromDebit { get; set; }

        [Display(Name = "Por Acreditar")]
        public decimal FromCredit { get; set; }

        [Display(Name = "Saldo")]
        public decimal Sold { get; set; }


        [Display(Name = "Cuanta Contable")]
        public int AccountId { get; set; }



        [JsonIgnore] 
        public virtual Account? Account { get; set; }

        [JsonIgnore] 
        public virtual State? State { get; set; }

        [JsonIgnore] 
        public virtual ICollection<BankParameter> BankParameters { get; set; } = [];
        
        [JsonIgnore] 
        public virtual ICollection<Acreditation> Acreditations { get; set; } = [];

        [JsonIgnore]
        public virtual ICollection<Debitation> Debitations { get; set; } = [];
    }
}
