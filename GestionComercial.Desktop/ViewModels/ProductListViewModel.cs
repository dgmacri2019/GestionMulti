using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Utils;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GestionComercial.Desktop.ViewModels
{
    public class ProductListViewModel : BaseViewModel
    {
        private readonly ProductApiService _productApiService;

        public ObservableCollection<ProductViewModel> Products { get; set; } = new ObservableCollection<ProductViewModel>();

        public ICommand LoadProductsCommand { get; }

        public ProductListViewModel()
        {
            _productApiService = new ProductApiService();
            //LoadProductsCommand = new RelayCommand(async () => await LoadProductsAsync());
           LoadProductsAsync();
        }

        public async Task LoadProductsAsync()
        {
            var products = await _productApiService.GetProductsWithPricesAsync();
            Products.Clear();
            foreach (var p in products)
            {
                Products.Add(p);
            }
        }
    }
}
