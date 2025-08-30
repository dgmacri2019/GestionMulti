using GestionComercial.Domain.Entities.Masters.Configuration;

namespace GestionComercial.Applications.Interfaces
{
    public interface IParameterService
    {
        Task<IEnumerable<GeneralParameter>> GetAllGeneralParametersAsync();
        Task<GeneralParameter?> GetGeneralParameterByIdAsync(int id);
    }
}
