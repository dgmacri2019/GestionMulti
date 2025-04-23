using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IArticleService
    {
        Task<GeneralResponse> AddAsync(Article product);
        Task<GeneralResponse> UpdateAsync(Article product);
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ArticleWithPricesDto>> GetAllAsync(bool isEnabled, bool isDeleted);
        Task<ArticleWithPricesDto?> GetByIdAsync(int id);
        Task<ArticleWithPricesDto?> FindByCodeOrBarCodeAsync(string code);
        Task<ArticleWithPricesDto?> FindByBarCodeAsync(string barCode);
        Task<IEnumerable<ArticleWithPricesDto>> SearchToListAsync(string description, bool isEnabled, bool isDeleted);
        Task<GeneralResponse> UpdatePricesAsync(IProgress<int> progress, int categoryId, int percentage);
        Task<ArticleResponse> GenerateNewBarCodeAsync();



        GeneralResponse Add(Article product);
        GeneralResponse Update(Article product);
        GeneralResponse Delete(int id);
        IEnumerable<ArticleWithPricesDto> GetAll(bool isEnabled, bool isDeleted);
        ArticleWithPricesDto? GetById(int id);
        ArticleWithPricesDto? FindByCodeOrBarCode(string code);
        ArticleWithPricesDto? FindByBarCode(string barCode);
        IEnumerable<ArticleWithPricesDto> SearchToList(string description, bool isEnabled, bool isDeleted);
        GeneralResponse UpdatePrices(IProgress<int> progress, int categoryId, int percentage);
        ArticleResponse GenerateNewBarCode();
    }
}
