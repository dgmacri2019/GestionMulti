using GestionComercial.Domain.DTOs.Stock;

namespace GestionComercial.Desktop.Cache
{
    internal class ArticleCache : ICache
    {
        private static ArticleCache _instance;
        public static ArticleCache Instance => _instance ??= new ArticleCache();

        private List<ArticleViewModel> _articles;

        private ArticleCache()
        {
            CacheManager.Register(this);
        }

        public List<ArticleViewModel> GetAllArticles()
        {
            return _articles;
        }

        public List<ArticleViewModel> SearchArticles(string name, bool isEnabled, bool isDeleted)
        {
            return _articles != null ? _articles
                .Where(a => a.IsEnabled == isEnabled && a.IsDeleted == isDeleted
                       && ((a.Description?.ToLower().Contains(name.ToLower()) ?? false)
                        || (a.Code?.ToLower().Contains(name.ToLower()) ?? false)
                        || (a.BarCode?.ToLower().Contains(name.ToLower()) ?? false)))
                .ToList()
                :
                [];
        }




        public void SetArticles(List<ArticleViewModel> articles)
        {
            _articles = articles;
        }


        public void ClearCache()
        {
            _articles.Clear();
        }
        public bool HasData => _articles != null && _articles.Any();
    }
}
