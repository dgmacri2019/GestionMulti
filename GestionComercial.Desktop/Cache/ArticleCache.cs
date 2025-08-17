using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionComercial.Desktop.Cache
{
    internal class ArticleCache
    {
        private static ArticleCache _instance;
        public static ArticleCache Instance => _instance ??= new ArticleCache();

        private List<ArticleWithPricesDto> _articles;

        public List<ArticleWithPricesDto> GetAllArticles()
        {
            return _articles;
        }

        public List<ArticleWithPricesDto> SearchArticles(string name, bool isEnabled, bool isDeleted)
        {
            return _articles
                    .Where(p => (p.Description?.ToLower().Contains(name.ToLower()) ?? false)
                              || (p.Code?.ToLower().Contains(name.ToLower()) ?? false)
                              || (p.BarCode?.ToLower().Contains(name.ToLower()) ?? false))
                    .ToList();
        }




        public void SetArticles(List<ArticleWithPricesDto> articles)
        {
            _articles= articles;
        }

        public bool HasData => _articles != null && _articles.Any();
    }
}
