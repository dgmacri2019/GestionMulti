using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows;

namespace GestionComercial.Desktop.Services
{
    internal class SalesApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public SalesApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }


        internal async Task<List<SaleViewModel>> GetAllAsync()
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/sales/GetAllAsync", new
                {
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });

                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    var sales = JsonSerializer.Deserialize<List<SaleViewModel>>(jsonResponse, options);


                    return sales;
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

        internal async Task<SaleResponse> GetAllBySalePointAsync(int salePoint)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/sales/GetAllBySalePointAsync", new
                {
                    SalePoint = salePoint,
                    SaleDate = DateTime.Now.Date,
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });

                string jsonResponse = await response.Content.ReadAsStringAsync();

                SaleResponse saleResponse = JsonSerializer.Deserialize<SaleResponse>(jsonResponse, options);
                if (response.IsSuccessStatusCode)
                    return saleResponse;
                else
                    return new SaleResponse
                    {
                        // Manejo de error
                        Message = saleResponse.Message,
                        Success = false,
                    };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    // Manejo de error
                    Message = ex.Message,
                    Success = false,
                };
            }
        }

        internal async Task<SaleResponse> GetByIdAsync(int saleId, string pcName)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/sales/GetByIdAsync", new
                {
                    Id = saleId,
                    PcName = pcName
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });


                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return new SaleResponse
                    {
                        SaleViewModel = JsonSerializer.Deserialize<SaleViewModel>(jsonResponse, options),
                        Success = true,
                    };
                else
                    return new SaleResponse
                    {
                        Success = false,
                        Message = $"Error: {response.StatusCode}\n{jsonResponse}",
                    };
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Sale sale)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("api/sales/UpdateAsync", sale);
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

        internal async Task<GeneralResponse> AddAsync(Sale sale)
        {
            try
            {
               var response = await _httpClient.PostAsJsonAsync("api/sales/AddAsync", sale);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<SaleResponse>(jsonResponse, options);
                //if (response.IsSuccessStatusCode)
                //    return saleResponse;
                //else
                //    return new SaleResponse
                //    {
                //        Success = false,
                //        Message = saleResponse.Message,
                //    };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

    }
}
