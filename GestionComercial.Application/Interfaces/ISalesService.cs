using GestionComercial.Domain.DTOs.Sale;

namespace GestionComercial.Applications.Interfaces
{
    public interface ISalesService
    {
        Task<IEnumerable<SaleViewModel>> GetAllAsync();
        Task<IEnumerable<SaleViewModel>> GetAllBySalePointAsync(int salePoint, DateTime saleDate);
        Task<SaleViewModel?> GetByIdAsync(int id);
    }
}
