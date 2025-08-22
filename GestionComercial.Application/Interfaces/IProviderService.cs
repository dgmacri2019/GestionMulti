using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IProviderService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ProviderViewModel>> GetAllAsync();
        //Task<IEnumerable<ProviderViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<ProviderViewModel?> GetByIdAsync(int id);
    }
}
