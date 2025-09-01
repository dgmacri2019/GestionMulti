using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class ParametersApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ParametersApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }

        internal async Task<List<GeneralParameter>> GetAllGeneralParametersAsync()
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/parameters/GetAllGeneralParametersAsync", new
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

                    List<GeneralParameter>? generalParameters = JsonSerializer.Deserialize<List<GeneralParameter>>(jsonResponse, options);


                    return generalParameters;
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

        internal async Task<List<PurchaseAndSalesListViewModel>> GetAllPcParametersAsync()
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/parameters/GetAllPcParametersAsync", new
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

                    List<PurchaseAndSalesListViewModel>? pcParameters = JsonSerializer.Deserialize<List<PurchaseAndSalesListViewModel>>(jsonResponse, options);


                    return pcParameters;
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

        internal async Task<PcParameter> GetPcParameterAsync(string pcName)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/parameters/GetPcParameterAsync", new
                {
                    PcName = pcName,
                    //IsEnabled = isEnabled,
                });

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    PcParameter? pcParameter = JsonSerializer.Deserialize<PcParameter>(jsonResponse, options);


                    return pcParameter;
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

        internal async Task<PcParameterResponse> GetPcParameterByIdAsync(int parameterId)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/parameters/GetPcParameterByIdAsync", new
                {
                    Id = parameterId,
                    //IsEnabled = isEnabled,
                });

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return new PcParameterResponse
                    {
                        Success = true,
                        PcParameter = JsonSerializer.Deserialize<PcParameter>(jsonResponse, options),
                    };
                else
                    return new PcParameterResponse
                    {
                        Success = false,
                        Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                    };

            }
            catch (Exception ex)
            {
                return new PcParameterResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdatePcParameterAsync(PcParameter pcParameter)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta
                var response = await _httpClient.PostAsJsonAsync("api/parameters/UpdatePcParameterAsync", pcParameter);
                var error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error: {response.StatusCode}\n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}
