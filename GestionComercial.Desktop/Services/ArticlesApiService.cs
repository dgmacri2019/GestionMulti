using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Stock;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    public class ArticlesApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ArticlesApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);

        }

        public async Task<List<ArticleWithPricesDto>> GetProductsWithPricesAsync()
        {
            // Llama al endpoint y deserializa la respuesta


            var response = await _httpClient.PostAsJsonAsync("api/articles/GetAllAsync", new
            {
                IsDeleted = false,
                IsEnabled = true,
            });

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var articles = JsonSerializer.Deserialize<List<ArticleWithPricesDto>>(jsonResponse, options);


                return articles;
            }
            else
            {
                // Manejo de error
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Error: {response.StatusCode}\n{error}");
                return null;
            }


        }
    }
}
