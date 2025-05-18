using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Purchases;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Entities.Stock;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Afip
{
    public class Tax : CommonEntity
    {
        [Display(Name = "Id Afip")]
        [MaxLength(5, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string AfipId { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(50, ErrorMessage = "El campo {0} no puede contener mas de {1} caracteres")]
        public string Description { get; set; }

        [Display(Name = "Impuesto")]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public decimal Rate { get; set; }






        [JsonIgnore] 
        public virtual ICollection<Article>? Products { get; set; }

        [JsonIgnore]
        public virtual ICollection<PurchaseDetail>? PurcheaseDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<PurchaseDetailTmp>? PurcheaseDetailTmps { get; set; }

        [JsonIgnore]
        public virtual ICollection<SaleDetailTmp>? SaleDetailTmps { get; set; }

        [JsonIgnore]
        public virtual ICollection<SaleDetail>? SaleDetails { get; set; }
    }
}
