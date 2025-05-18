using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Sales
{
    public class Reservation : CommonEntity
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Cliente")]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Vanta")]
        public int SaleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Fecha de venta")]
        public DateTime ReservationDate { get; set; }

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

        public int ReservationNumber { get; set; }

        public int ReservationPoint { get; set; }

        public string ReservationNumberString => string.Format("{0:00000}-{1:00000000}", ReservationPoint, ReservationNumber);




        [JsonIgnore] 
        public virtual Client? Client { get; set; }

        [JsonIgnore] 
        public virtual Sale? Sale { get; set; }

        [JsonIgnore] 
        public virtual ICollection<ReservationDetail>? ReservationDetails { get; set; }

        [JsonIgnore] 
        public virtual ICollection<ReservationPayMetodDetail>? ReservationPayMetodDetails { get; set; }

        [JsonIgnore] 
        public virtual ICollection<Acreditation>? Acreditations { get; set; }
    }
}
