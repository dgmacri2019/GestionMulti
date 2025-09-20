using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;

namespace Afip.PublicServices.Interfaces
{
    public interface IWSFEHomologacionService
    {
        Task<InvoiceResponse> SolicitarCAEAsync(Invoice invoice, int invoiceAnularId);
        Task<InvoiceResponse> GetLastCbteAsync(int ptoVta, int cbteTipo);
        Task<InvoiceResponse> ConsultarComprobanteAsync(long cbteNro, int ptoVta, int cbteTipo);
    }
}
