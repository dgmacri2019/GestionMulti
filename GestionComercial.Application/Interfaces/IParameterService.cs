using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;

namespace GestionComercial.Applications.Interfaces
{
    public interface IParameterService
    {
        Task<GeneralParameter?> GetGeneralParameterAsync();
        Task<GeneralParameter?> GetGeneralParameterByIdAsync(int id);



        Task<PcParameter?> GetPcParameterAsync(string pcName);
        Task<PcParameter?> GetPcParameterByIdAsync(int id);
        Task<IEnumerable<PcSalePointsListViewModel>> GetAllPcParametersAsync();



        Task<PcPrinterParametersListViewModel?> GetPrinterParameterFromPcAsync(string pcName);
        Task<IEnumerable<PcPrinterParametersListViewModel>> GetAllPcPrinterParametersAsync();


        Task<EmailParameter?> GetEmailParameterAsync();


    }
}
