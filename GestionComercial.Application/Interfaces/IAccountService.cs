using GestionComercial.Domain.DTOs.Accounts;

namespace GestionComercial.Applications.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountViewModel>> GetAllAsync(bool isEnabled, bool isDeleted, bool all);
        Task<AccountViewModel?> GetByIdAsync(int id);
        Task<IEnumerable<AccountViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
    }
}
