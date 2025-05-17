using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IArticleService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ArticleWithPricesDto>> GetAllAsync(bool isEnabled, bool isDeleted);
        Task<ArticleViewModel?> GetByIdAsync(int id);
        Task<ArticleWithPricesDto?> FindByCodeOrBarCodeAsync(string code);
        Task<ArticleWithPricesDto?> FindByBarCodeAsync(string barCode);
        Task<IEnumerable<ArticleWithPricesDto>> SearchToListAsync(string description, bool isEnabled, bool isDeleted);
        Task<GeneralResponse> UpdatePricesAsync(IProgress<int> progress, int categoryId, int percentage);
        Task<ArticleResponse> GenerateNewBarCodeAsync();

    }
}
