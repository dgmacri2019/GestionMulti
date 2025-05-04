using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
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



        internal async Task<List<ArticleWithPricesDto>> GetProductsWithPricesAsync(bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/articles/GetAllAsync", new
            {
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
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

        internal async Task<ArticleResponse> GetByIdAsync(int articleId, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/articles/GetByIdAsync", new
            {
                Id = articleId,
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ArticleResponse
                {
                    ArticleViewModel = JsonSerializer.Deserialize<ArticleViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new ArticleResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<List<ArticleWithPricesDto>> SearchToListAsync(string description, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/articles/SearchToListAsync", new
            {
                Description = description,
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
            });

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

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
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Article article)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/articles/UpdateAsync", article);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> AddAsync(Article article)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/articles/AddAsync", article);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
    }
}
