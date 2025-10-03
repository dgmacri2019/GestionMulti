using GestionComercial.Domain.Entities.Sales;

namespace GestionComercial.Domain.Response
{
    public class InvoiceResponse : GeneralResponse
    {
        public Invoice? Invoice { get; set; }
        public int LastCbte { get; set; }
        public int ErrorCode { get; set; }
        public string? CAE { get; set; }
        public long CompNro { get; set; }
        public string? FechaVtoCAE { get; set; }
        public string? FechaProceso { get; set; }
        public string? Resultado { get; set; }
        public List<Invoice>? Invoices { get; set; }
        public byte[]? Bytes { get; set; }
    }
}
