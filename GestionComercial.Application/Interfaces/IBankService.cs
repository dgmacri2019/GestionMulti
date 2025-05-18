using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IBankService
    {
        Task<GeneralResponse> DeleteBankAsync(int id);

        Task<GeneralResponse> DeleteBoxAsync(int id);

        Task<GeneralResponse> DeleteBankParamerAsync(int id);

        Task<GeneralResponse> DeleteAcreditationAsync(int id);

        Task<GeneralResponse> DeleteDebitationAsync(int id);








        Task<BankViewModel?> GetBankByIdAsync(int id);       
        Task<IEnumerable<BankAndBoxViewModel>> SearchBankAndBoxToListAsync(string name, bool isEnabled, bool isDeleted);
        Task<BoxViewModel?> GetBoxByIdAsync(int id);





        Task<BankParameterViewModel?> GetBankParameterByIdAsync(int id);
        Task<IEnumerable<BankParameterViewModel>> SearchBankParameterToListAsync(string name, bool isEnabled, bool isDeleted);

    }
}
