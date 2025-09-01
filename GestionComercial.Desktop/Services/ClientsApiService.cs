using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class ClientsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ClientsApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }



        internal async Task<List<ClientViewModel>> SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/clients/SearchToListAsync", new
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

                    var articles = JsonSerializer.Deserialize<List<ClientViewModel>>(jsonResponse, options);


                    return articles;
                }
                else
                {
                    // Manejo de error
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal async Task<List<ClientViewModel>> GetAllAsync()
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/clients/GetAllAsync", new
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

                    var articles = JsonSerializer.Deserialize<List<ClientViewModel>>(jsonResponse, options);


                    return articles;
                }
                else
                {
                    // Manejo de error
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                    return null;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal async Task<ClientResponse> GetByIdAsync(int clientId)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/clients/GetByIdAsync", new
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
                    return new ClientResponse
                    {
                        ClientViewModel = JsonSerializer.Deserialize<ClientViewModel>(jsonResponse, options),
                        Success = true,
                    };
                else
                    return new ClientResponse
                    {
                        Success = false,
                        Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                    };
            }
            catch (Exception ex)
            {
                return new ClientResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Client client)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/clients/UpdateAsync", client);
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error: {response.StatusCode}\n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal async Task<GeneralResponse> AddAsync(Client client)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/clients/AddAsync", client);
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error: {response.StatusCode}\n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
