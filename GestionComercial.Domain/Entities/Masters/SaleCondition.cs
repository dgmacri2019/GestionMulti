using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Sales;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Masters
{
    public class SaleCondition : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Descripción")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        public required string Description { get; set; }

        public int AfipId { get; set; }

        [Display(Name = "Descripción Corta")]
        [MaxLength(10, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        public string? SmallDescription { get; set; }

        public int BoxId { get; set; }

        public int BankParameterId { get; set; }


        //[JsonIgnore]
        public virtual BankParameter? BankParameter { get; set; }
        public virtual Box? Box { get; set; }
        //public virtual ICollection<Client>? Clients { get; set; }
        public virtual ICollection<Provider>? Providers { get; set; }
        public virtual ICollection<SalePayMetodDetail>? SalePayMetodDetails { get; set; }
    }
}
