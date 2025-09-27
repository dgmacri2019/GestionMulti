using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;
using System.Xml.Linq;

namespace GestionComercial.Desktop.Services
{
    internal class ParametersApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        private readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public ParametersApiService()
        {
            _apiService = new ApiService();
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
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
                    List<GeneralParameter>? generalParameters = JsonSerializer.Deserialize<List<GeneralParameter>>(jsonResponse, Options);

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

        internal async Task<List<PcSalePointsListViewModel>> GetAllPcParametersAsync()
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
                    List<PcSalePointsListViewModel>? pcParameters = JsonSerializer.Deserialize<List<PcSalePointsListViewModel>>(jsonResponse, Options);

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

                    PcParameter? pcParameter = JsonSerializer.Deserialize<PcParameter>(jsonResponse, Options);


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

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return new PcParameterResponse
                    {
                        Success = true,
                        PcParameter = JsonSerializer.Deserialize<PcParameter>(jsonResponse, Options),
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


        internal async Task<List<PcPrinterParametersListViewModel>> GetAllPcPrinterParametersAsync()
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/parameters/GetAllPcPrinterParametersAsync", new
                {
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<List<PcPrinterParametersListViewModel>>(jsonResponse, Options);
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

        internal async Task<PcPrinterParametersListViewModel?> GetPrinterParameterFromPcAsync(string pcName)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/parameters/GetPrinterParameterFromPcAsync", new
                {
                    PcName = pcName,
                    //IsEnabled = isEnabled,
                });

                var jsonResponse = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(jsonResponse))
                    return null;
                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<PcPrinterParametersListViewModel>(jsonResponse, Options);
                }
                else
                {
                    // Manejo de error
                    MessageBox.Show($"Error: {response.StatusCode}\n{jsonResponse}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);
                return null;
            }
        }

        internal async Task<GeneralResponse> UpdatePcPrinterParameterAsync(PrinterParameter printerParameter)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta
                var response = await _httpClient.PostAsJsonAsync("api/parameters/UpdatePcPrinterParameterAsync", printerParameter);
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
