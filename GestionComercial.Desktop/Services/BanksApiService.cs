using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class BanksApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public BanksApiService(string superToken = "")
        {
            _apiService = string.IsNullOrEmpty(superToken) ? new ApiService("api/banks/") :new ApiService("api/caches/banks/");
            string token = LoginUserCache.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }

        internal async Task<List<BankAndBoxViewModel>> SearchAsync()
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("SearchBankAndBoxToListAsync", new
            {
                //Name = name,
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

                return JsonSerializer.Deserialize<List<BankAndBoxViewModel>>(jsonResponse, options);

            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<BankAndBoxResponse> GetBankByIdAsync(int bankId, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("GetBankByIdAsync", new
            {
                Id = bankId,
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new BankAndBoxResponse
                {
                    BankViewModel = JsonSerializer.Deserialize<BankViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new BankAndBoxResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<BankAndBoxResponse> GetBoxByIdAsync(int boxId, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("GetBoxByIdAsync", new
            {
                Id = boxId,
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new BankAndBoxResponse
                {
                    BoxViewModel = JsonSerializer.Deserialize<BoxViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new BankAndBoxResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<GeneralResponse> UpdateBankAsync(Bank bank)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("UpdateBankAsync", bank);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> AddBankAsync(Bank bank)
        {

            var response = await _httpClient.PostAsJsonAsync("AddBankAsync", bank);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> AddBoxAsync(Box box)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("AddBoxAsync", box);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> UpdateBoxAsync(Box box)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("UpdateBoxAsync", box);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }


        internal async Task<List<BankParameterViewModel>> SearchBankParameterAsync()
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("SearchBankParameterToListAsync", new
            {
                //Name = name,
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

                return JsonSerializer.Deserialize<List<BankParameterViewModel>>(jsonResponse, options);

            }
            else
            {
                // Manejo de error
                MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                return null;
            }
        }

        internal async Task<BankAndBoxResponse> GetBankParameterByIdAsync(int bankParameterId, bool isEnabled, bool isDeleted)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("GetBankParameterByIdAsync", new
            {
                Id = bankParameterId,
                IsDeleted = isDeleted,
                IsEnabled = isEnabled,
            });

            JsonSerializerOptions options = new()
            {
                PropertyNameCaseInsensitive = true
            };

            var jsonResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return new BankAndBoxResponse
                {
                    BankParameterViewModel = JsonSerializer.Deserialize<BankParameterViewModel>(jsonResponse, options),
                    Success = true,
                };
            else
                return new BankAndBoxResponse
                {
                    Success = false,
                    Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                };
        }

        internal async Task<GeneralResponse> AddBankParameterAsync(BankParameter bankParameter)
        {

            var response = await _httpClient.PostAsJsonAsync("AddBankParameterAsync", bankParameter);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> UpdateBankParameterAsync(BankParameter bankParameter)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("UpdateBankParameterAsync", bankParameter);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
    }
}
