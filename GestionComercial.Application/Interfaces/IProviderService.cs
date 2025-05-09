using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IProviderService
    {
        Task<GeneralResponse> AddAsync(Provider client);
        Task<GeneralResponse> UpdateAsync(Provider client);
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ProviderViewModel>> GetAllAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<ProviderViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<ProviderViewModel?> GetByIdAsync(int id, bool isEnabled, bool isDeleted);
    }
}
