using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;

namespace GestionComercial.Applications.Interfaces
{
    public interface IMasterClassService
    {
        Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<IvaCondition>> GetAllIvaConditionsAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<SaleCondition>> GetAllSaleConditionsAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<State>> GetAllStatesAsync(bool isEnabled, bool isDeleted);
    }
}
