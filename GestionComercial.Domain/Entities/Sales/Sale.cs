using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Sales
{
    public class Sale : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de venta")]
        public DateTime SaleDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Forma de pago")]
        public SaleCondition SaleCondition { get; set; }

        [Display(Name = "Importe total de la venta")]
        public decimal Total { get; set; }

        public decimal SubTotal { get; set; }

        public decimal GeneralDiscount { get; set; }

        public decimal InternalTax { get; set; }

        public decimal TotalIVA21 { get; set; }

        public decimal TotalIVA105 { get; set; }

        public decimal TotalIVA27 { get; set; }

        public decimal BaseImp21 { get; set; }

        public decimal BaseImp105 { get; set; }

        public decimal BaseImp27 { get; set; }

        [Display(Name = "Saldo Pendiente")]
        public decimal Sold { get; set; }

        public bool PaidOut { get; set; }

        public bool PartialPay { get; set; }

        public bool IsFinished { get; set; }

        public int AutorizationCode { get; set; }

        public int SaleNumber { get; set; }

        public int SalePoint { get; set; }

        public string SaleNumberString => string.Format("{0:00000}-{1:00000000}", SalePoint, SaleNumber);





        [JsonIgnore]
        public virtual Client? Client { get; set; }

        [JsonIgnore]
        public virtual ICollection<SaleDetail>? SaleDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<SalePayMetodDetail>? SalePayMetodDetails { get; set; }

        [JsonIgnore]
        public virtual ICollection<Acreditation>? Acreditations { get; set; }

    }
}
