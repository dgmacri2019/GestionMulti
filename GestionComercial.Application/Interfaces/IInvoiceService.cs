using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceResponse> FindByIdAsync(int id);
        Task<InvoiceResponse> FindBySaleIdAsync(int saleId, int compTypeId);
        Task<InvoiceResponse> GetAllAsync(int page, int pageSize);
        Task<InvoiceResponse> GetAllBySalePointAsync(int salePoint, DateTime saleDate, int page, int pageSize);
       
    }
}
