using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Response;
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
            string token = LoginUserCache.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
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

        internal async Task<AccountResponse> GetByIdAsync(int id)
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
                return new AccountResponse
                {
                    AccountViewModel = JsonSerializer.Deserialize<AccountViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new AccountResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<List<AccountType>> GetAllAccountTypesAsync(bool isEnabled, bool isDeleted, bool all)
        {
            var response = await _httpClient.PostAsJsonAsync("api/accounts/GetAllAccountTypesAsync", new
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

                return JsonSerializer.Deserialize<List<AccountType>>(jsonResponse, options);
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<List<Account>> GetAllAccountAsync(bool isEnabled, bool isDeleted, bool all)
        {
            var response = await _httpClient.PostAsJsonAsync("api/accounts/GetAllAccountsAsync", new
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

                return JsonSerializer.Deserialize<List<Account>>(jsonResponse, options);
            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Account account)
        {
            var response = await _httpClient.PostAsJsonAsync("api/accounts/UpdateAccountAsync", account);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> AddAsync(Account account)
        {
            var response = await _httpClient.PostAsJsonAsync("api/accounts/AddAccountAsync", account);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
    }
}
