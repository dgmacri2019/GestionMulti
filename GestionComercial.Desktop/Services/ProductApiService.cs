using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace GestionComercial.Desktop.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        
        public ProductApiService()
        {
            _apiService = new ApiService();
            
            _httpClient = _apiService.GetHttpClient();
        }

        public async Task<List<ProductViewModel>> GetProductsWithPricesAsync()
        {
            // Llama al endpoint y deserializa la respuesta
            List<ProductViewModel> products = await _httpClient.GetFromJsonAsync<List<ProductViewModel>>("api/products/with-prices");
            return products ?? new List<ProductViewModel>();
        }
    }
}
