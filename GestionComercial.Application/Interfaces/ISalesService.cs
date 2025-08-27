using GestionComercial.Domain.DTOs.Sale;

namespace GestionComercial.Applications.Interfaces
{
    public interface ISalesService
    {
        Task<IEnumerable<SaleViewModel>> GetAllAsync();
        Task<SaleViewModel?> GetByIdAsync(int id);
    }
}
