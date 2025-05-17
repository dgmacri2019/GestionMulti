using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IPriceListService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<PriceListViewModel>> GetAllAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<PriceListViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<PriceListViewModel?> GetByIdAsync(int id, bool isEnabled, bool isDeleted);
    }
}
