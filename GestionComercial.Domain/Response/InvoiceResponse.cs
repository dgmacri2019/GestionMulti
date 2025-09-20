using GestionComercial.Domain.Entities.Sales;

namespace GestionComercial.Domain.Response
{
    public class InvoiceResponse : GeneralResponse
    {
        public Invoice Invoice { get; set; }
        public int LastCbte { get; set; }
        public int ErrorCode { get; set; }
    }
}
