using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IArticleService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<ArticleResponse> GetAllAsync(int page, int pageSize);
        Task<ArticleViewModel?> GetByIdAsync(int id);
        Task<GeneralResponse> UpdatePricesAsync(IProgress<int> progress, int categoryId, int percentage);
        Task<ArticleResponse> GenerateNewBarCodeAsync();

    }
}
