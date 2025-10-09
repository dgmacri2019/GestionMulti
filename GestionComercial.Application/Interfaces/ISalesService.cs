using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface ISalesService
    {
        Task<SaleResponse> AddAsync(Sale sale);
        Task<SaleResponse> AnullAsync(Sale sale, string userName);
        Task<SaleResponse> GetAllAsync(int page, int pageSize);
        Task<SaleResponse> GetAllBySalePointAsync(int salePoint, DateTime saleDate, int page, int pageSize);
        Task<SaleResponse> GetByIdAsync(int id);
        Task<SaleResponse> GetLastSaleNumber(int salePoint);
    }
}
