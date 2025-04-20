using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IArticleService
    {
        Task<GeneralResponse> AddAsync(Article product);
        Task<GeneralResponse> UpdateAsync(Article product);
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ProductWithPricesDto>> GetAllAsync(bool isEnabled, bool isDeleted);
        Task<ProductWithPricesDto?> GetByIdAsync(int id);
        Task<ProductWithPricesDto?> FindByCodeOrBarCodeAsync(string code);
        Task<ProductWithPricesDto?> FindByBarCodeAsync(string barCode);
        Task<IEnumerable<ProductWithPricesDto>> SearchToListAsync(string description, bool isEnabled, bool isDeleted);
        Task<GeneralResponse> UpdatePricesAsync(IProgress<int> progress, int categoryId, int percentage);
        Task<ProductResponse> GenerateNewBarCodeAsync();



        GeneralResponse Add(Article product);
        GeneralResponse Update(Article product);
        GeneralResponse Delete(int id);
        IEnumerable<ProductWithPricesDto> GetAll(bool isEnabled, bool isDeleted);
        ProductWithPricesDto? GetById(int id);
        ProductWithPricesDto? FindByCodeOrBarCode(string code);
        ProductWithPricesDto? FindByBarCode(string barCode);
        IEnumerable<ProductWithPricesDto> SearchToList(string description, bool isEnabled, bool isDeleted);
        GeneralResponse UpdatePrices(IProgress<int> progress, int categoryId, int percentage);
        ProductResponse GenerateNewBarCode();
    }
}
