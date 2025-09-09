using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IClientService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ClientViewModel>> GetAllAsync(int page, int pageSize);

        //Task<IEnumerable<ClientViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<ClientViewModel?> GetByIdAsync(int id);
    }
}
