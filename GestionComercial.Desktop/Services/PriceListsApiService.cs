using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class PriceListsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public PriceListsApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }


        internal async Task<List<PriceListViewModel>> SearchAsync(string description, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/PriceLists/SearchToListAsync", new
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

                return JsonSerializer.Deserialize<List<PriceListViewModel>>(jsonResponse, options);

            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<List<PriceListViewModel>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/PriceLists/GetAllAsync", new
            {
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

                return JsonSerializer.Deserialize<List<PriceListViewModel>>(jsonResponse, options);

            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<PriceListResponse> GetByIdAsync(int pricelistId, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/PriceLists/GetByIdAsync", new
            {
                Id = pricelistId,
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new PriceListResponse
                {
                    PriceListViewModel = JsonSerializer.Deserialize<PriceListViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new PriceListResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<GeneralResponse> UpdateAsync(PriceList priceList)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/PriceLists/UpdateAsync", priceList);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> AddAsync(PriceList priceList)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/PriceLists/AddAsync", priceList);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

    }
}
