using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;

namespace GestionComercial.Applications.Interfaces
{
    public interface IParameterService
    {
        Task<IEnumerable<GeneralParameter>> GetAllGeneralParametersAsync();
        Task<IEnumerable<PurchaseAndSalesListViewModel>> GetAllPcParametersAsync();
        Task<GeneralParameter?> GetGeneralParameterByIdAsync(int id);

        Task<PcParameter?> GetPcParameterAsync(string pcName);
        Task<PcParameter?> GetPcParameterByIdAsync(int id);
    }
}
