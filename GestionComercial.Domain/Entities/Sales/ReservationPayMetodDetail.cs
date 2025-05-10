using GestionComercial.Domain.Entities.Masters;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Entities.Sales
{
    public class ReservationPayMetodDetail : CommonEntity
    {
        public int ReservationId { get; set; }

        public SaleCondition SaleCondition { get; set; }

        public decimal Value { get; set; }

        public virtual Reservation Reservation { get; set; }
    }
}
