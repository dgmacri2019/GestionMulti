using GestionComercial.Domain.Entities.Masters;
using System.Text.Json.Serialization;

namespace GestionComercial.Domain.Entities.Sales
{
    public class InvoiceDetail : CommonEntity
    {
        public int InvoiceId { get; set; }

        public int IvaId { get; set; }

        public double BaseImpIva { get; set; }

        public double ImporteIva { get; set; }



        [JsonIgnore] 
        public virtual Invoice? Invoice { get; set; }
    }
}
