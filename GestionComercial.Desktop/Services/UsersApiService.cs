using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.User;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class UsersApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public UsersApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
        }

        internal async Task<List<UserViewModel>> GetAllAsync(bool isEnabled, bool all)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/users/GetAllAsync", new
            {
                IsEnabled = isEnabled,
                All = all
            });

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<UserViewModel>>(jsonResponse, options);
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }
        internal async Task<List<UserViewModel>> SearchToListAsync(string name, bool isEnabled)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("api/users/SearchToListAsync", new
            {
                IsEnabled = isEnabled,
                NameFilter = name
            });

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                return JsonSerializer.Deserialize<List<UserViewModel>>(jsonResponse, options);
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }
    }
}
