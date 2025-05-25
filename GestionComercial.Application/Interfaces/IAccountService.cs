using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.Entities.AccountingBook;

namespace GestionComercial.Applications.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountViewModel>> GetAllAsync(bool isEnabled, bool isDeleted, bool all);
        Task<AccountViewModel?> GetByIdAsync(int id);
        Task<IEnumerable<AccountViewModel>> SearchToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<IEnumerable<AccountType>> GetAllAccountTypesAsync(bool isEnabled, bool isDeleted, bool all);
        Task<IEnumerable<Account>> GetAllAccountsAsync(bool isEnabled, bool isDeleted, bool all);
        Task<IEnumerable<Account>> GetAccountGroup1Async(int accountType, bool isEnabled, bool isDeleted, bool all);
        Task<IEnumerable<Account>> GetAccountGroup2Async(int accountType, int accountGroup1, bool isEnabled, bool isDeleted, bool all);
        Task<IEnumerable<Account>> GetAccountGroup3Async(int accountType, int accountGroup1, int accountGroup2, bool isEnabled, bool isDeleted, bool all);
        Task<IEnumerable<Account>> GetAccountGroup4Async(int accountType, int accountGroup1, int accountGroup2, int accountGroup3, bool isEnabled, bool isDeleted, bool all);
        Task<IEnumerable<Account>> GetAccountGroup5Async(int accountType, int accountGroup1, int accountGroup2, int accountGroup3, int accountGroup4, bool isEnabled, bool isDeleted, bool all);
    }
}
