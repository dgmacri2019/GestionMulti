using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Stock;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GestionComercial.Desktop.ViewModels.Stock
{
    public class ArticleListViewModel : BaseViewModel
    {
        private readonly ArticlesApiService _articlesApiService;

        public ObservableCollection<ArticleWithPricesDto> Products { get; set; } = [];

        public ICommand? LoadProductsCommand { get; }

        public ArticleListViewModel(bool isEnabled, bool isDeleted)
        {
            _articlesApiService = new ArticlesApiService();
            //LoadProductsCommand = new RelayCommand(async () => await LoadProductsAsync());
            GetAllArticlesAsync(isEnabled, isDeleted);
        }

        public ArticleListViewModel(string description, bool isEnabled, bool isDeleted)
        {
            _articlesApiService = new ArticlesApiService();
            //LoadProductsCommand = new RelayCommand(async () => await LoadProductsAsync());
            SearchAsync(description, isEnabled, isDeleted);
        }

        public async Task GetAllArticlesAsync(bool isEnabled, bool isDeleted)
        {
            List<ArticleWithPricesDto> products = await _articlesApiService.GetProductsWithPricesAsync(isEnabled, isDeleted);
            Products.Clear();
            foreach (var p in products)
            {
                Products.Add(p);
            }
        }


        public async Task<ObservableCollection<ArticleWithPricesDto>> SearchAsync(string description, bool isEnabled, bool isDeleted)
        {
            List<ArticleWithPricesDto> products = await _articlesApiService.SearchToListAsync(description, isEnabled, isDeleted);
            Products.Clear();
            foreach (var p in products)
            {
                Products.Add(p);
            }

            return Products;
        }




    }
}