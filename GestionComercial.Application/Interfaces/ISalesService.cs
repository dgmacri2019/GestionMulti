using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface ISalesService
    {
        Task<SaleResponse> AddAsync(Sale sale);
        Task<IEnumerable<SaleViewModel>> GetAllAsync();
        Task<IEnumerable<SaleViewModel>> GetAllBySalePointAsync(int salePoint, DateTime saleDate);
        Task<SaleViewModel?> GetByIdAsync(int id);
        Task<int> GetLastSaleNumber(int salePoint);
    }
}
