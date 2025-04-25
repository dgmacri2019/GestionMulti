using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Stock;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GestionComercial.Desktop.ViewModels.Stock
{
    public class ArticleListViewModel : BaseViewModel
    {
        private readonly ArticlesApiService _articlesApiService;

        public ObservableCollection<ArticleWithPricesDto> Products { get; set; } = new ObservableCollection<ArticleWithPricesDto>();

        public ICommand LoadProductsCommand { get; }

        public ArticleListViewModel()
        {
            _articlesApiService = new ArticlesApiService();
            //LoadProductsCommand = new RelayCommand(async () => await LoadProductsAsync());
            LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            var products = await _articlesApiService.GetProductsWithPricesAsync();
            Products.Clear();
            foreach (var p in products)
            {
                Products.Add(p);
            }
        }
    }
}
