using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class ProvidersApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ProvidersApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }

        internal async Task<List<ProviderViewModel>> SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/providers/SearchToListAsync", new
            {
                Name = name,
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

                var providers = JsonSerializer.Deserialize<List<ProviderViewModel>>(jsonResponse, options);


                return providers;
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<List<ProviderViewModel>> GetAllAsync()
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/providers/GetAllAsync", new
            {
                //IsDeleted = isDeleted,
                //IsEnabled = isEnabled,
            });

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                var providers = JsonSerializer.Deserialize<List<ProviderViewModel>>(jsonResponse, options);


                return providers;
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<ProviderResponse> GetByIdAsync(int clientId)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/providers/GetByIdAsync", new
            {
                Id = clientId,
                //IsDeleted = isDeleted,
                //IsEnabled = isEnabled,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new ProviderResponse
                {
                    ProviderViewModel = JsonSerializer.Deserialize<ProviderViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new ProviderResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<GeneralResponse> UpdateAsync(Provider provider)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/providers/UpdateAsync", provider);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> AddAsync(Provider provider)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/providers/AddAsync", provider);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
    }
}
