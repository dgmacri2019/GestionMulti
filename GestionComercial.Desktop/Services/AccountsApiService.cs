using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.DTOs.Client;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class AccountsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public AccountsApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
        }


        internal async Task<List<AccountViewModel>> SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/accounts/SearchToListAsync", new
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

                return JsonSerializer.Deserialize<List<AccountViewModel>>(jsonResponse, options);
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<List<AccountViewModel>> GetAllAsync(bool isEnabled, bool isDeleted, bool all)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/accounts/GetAllAsync", new
            {
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
                All = all
            });

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<AccountViewModel>>(jsonResponse, options);
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<AccountViewModel> GetByIdAsync(int id)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/accounts/GetByIdAsync", new
            {
                Id = id,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };
            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonSerializer.Deserialize<AccountViewModel>(jsonResponse, options);
            else
                return new AccountViewModel
                {
                    Id = 0
                };
        }

    }
}
