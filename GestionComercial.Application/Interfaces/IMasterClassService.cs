using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;

namespace GestionComercial.Applications.Interfaces
{
    public interface IMasterClassService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);


        Task<IEnumerable<PriceList>> GetAllPriceListAsync();
        Task<PriceList?> GetPriceListByIdAsync(int id);


        Task<IEnumerable<DocumentType>> GetAllDocumentTypesAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<IvaCondition>> GetAllIvaConditionsAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<Measure>> GetAllMeasuresAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<SaleCondition>> GetAllSaleConditionsAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<State>> GetAllStatesAsync(bool isEnabled, bool isDeleted);
        Task<IEnumerable<Tax>> GetAllTaxesAsync(bool isEnabled, bool isDeleted);

    }
}
