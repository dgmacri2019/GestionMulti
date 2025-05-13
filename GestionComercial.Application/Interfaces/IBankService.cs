using GestionComercial.Domain.DTOs.Bank;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IBankService
    {
        Task<GeneralResponse> AddBankAsync(Bank bank);
        Task<GeneralResponse> UpdateBankAsync(Bank bank);
        Task<GeneralResponse> DeleteBankAsync(int id);

        Task<GeneralResponse> AddBoxAsync(Box box);
        Task<GeneralResponse> UpdateBoxAsync(Box box);
        Task<GeneralResponse> DeleteBoxAsync(int id);

        Task<GeneralResponse> AddBankParamerAsync(BankParameter bankParameter);
        Task<GeneralResponse> UpdateBankParamerAsync(BankParameter bankParameter);
        Task<GeneralResponse> DeleteBankParamerAsync(int id);

        Task<GeneralResponse> AddAcreditationAsync(Acreditation acreditation);
        Task<GeneralResponse> UpdateAcreditationAsync(Acreditation acreditation);
        Task<GeneralResponse> DeleteAcreditationAsync(int id);

        Task<GeneralResponse> AddDebitationAsync(Debitation debitation);
        Task<GeneralResponse> UpdateDebitationAsync(Debitation debitation);
        Task<GeneralResponse> DeleteDebitationAsync(int id);








        Task<BankViewModel?> GetBankByIdAsync(int id);
        Task<IEnumerable<BankAndBoxViewModel>> SearchBankAndBoxToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<BoxViewModel?> GetBoxByIdAsync(int id);







    }
}
