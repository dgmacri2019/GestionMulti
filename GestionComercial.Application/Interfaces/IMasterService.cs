using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IMasterService
    {
        Task<GeneralResponse> AddAsync<T>(T model);
        Task<GeneralResponse> UpdateAsync<T>(T model);
        Task<GeneralResponse> DeleteAsync<T>(T model);
    }
}
