using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IArticleService
    {
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ArticleViewModel>> GetAllAsync();
        Task<ArticleViewModel?> GetByIdAsync(int id);
        Task<ArticleViewModel?> FindByCodeOrBarCodeAsync(string code);
        Task<ArticleViewModel?> FindByBarCodeAsync(string barCode);
        Task<IEnumerable<ArticleViewModel>> SearchToListAsync(string description);
        Task<GeneralResponse> UpdatePricesAsync(IProgress<int> progress, int categoryId, int percentage);
        Task<ArticleResponse> GenerateNewBarCodeAsync();

    }
}
