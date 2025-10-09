using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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
        public SalesApiService(string superToken = "")
        {
            _apiService = string.IsNullOrEmpty(superToken) ? new ApiService("api/sales/") : new ApiService("api/caches/sales/");
            string token = LoginUserCache.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }


        internal async Task<SaleResponse> GetAllAsync(int pageSize = 100)
        {
            // Llama al endpoint y deserializa la respuesta
            List<SaleViewModel> allSales = [];
            int page = 1;
            bool moreData = true;
            SaleResponse saleResponse = new()
            {
                Success = false,
            };
            try
            {
                while (moreData)
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("GetAllAsync", new
                    {
                        Page = page,
                        PageSize = pageSize
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // Leer el contenido como stream para no cargar todo en memoria
                        using Stream? stream = await response.Content.ReadAsStreamAsync();


                        SaleResponse? result = JsonSerializer.Deserialize<SaleResponse>(stream, options);
                        if (result.Success)
                        {
                            if (result.SaleViewModels == null || result.SaleViewModels.Count() == 0)
                            {
                                moreData = false; // no quedan más datos
                            }
                            else
                            {
                                allSales.AddRange(result.SaleViewModels);
                                page++; // siguiente página
                            }
                        }
                    }
                    else
                    {
                        saleResponse.Message = await response.Content.ReadAsStringAsync();
                        return saleResponse;
                    }
                }
                saleResponse.Success = true;
                saleResponse.SaleViewModels = allSales;
                return saleResponse;

            }
            catch (Exception ex)
            {
                saleResponse.Message = $"Error al obtener ventas, el error fue:\n {ex.Message}";
                return saleResponse;
            }
        }

        internal async Task<SaleResponse> GetAllBySalePointAsync(int salePoint, int pageSize = 100)
        {
            // Llama al endpoint y deserializa la respuesta
            List<SaleViewModel> allSales = [];
            int page = 1;
            bool moreData = true;
            SaleResponse saleResponse = new()
            {
                Success = false,
            };
            try
            {
                while (moreData)
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("GetAllBySalePointAsync", new
                    {
                        SalePoint = salePoint,
                        SaleDate = DateTime.Now.Date,
                        Page = page,
                        PageSize = pageSize
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // Leer el contenido como stream para no cargar todo en memoria
                        using Stream? stream = await response.Content.ReadAsStreamAsync();


                        SaleResponse? result = JsonSerializer.Deserialize<SaleResponse>(stream, options);
                        if (result.Success)
                        {
                            if (result.SaleViewModels == null || result.SaleViewModels.Count() == 0)
                            {
                                moreData = false; // no quedan más datos
                                saleResponse.LastSaleNumber = result.LastSaleNumber;
                            }
                            else
                            {
                                allSales.AddRange(result.SaleViewModels);
                                saleResponse.LastSaleNumber = result.LastSaleNumber;
                                page++; // siguiente página
                            }
                        }
                    }
                    else
                    {
                        saleResponse.Message = await response.Content.ReadAsStringAsync();
                        return saleResponse;
                    }
                }
                saleResponse.Success = true;
                saleResponse.SaleViewModels = allSales;
                return saleResponse;
            }
            catch (Exception ex)
            {
                saleResponse.Message = $"Error al obtener ventas, el error fue:\n {ex.Message}";
                return saleResponse;
            }
        }

        internal async Task<SaleResponse> GetByIdAsync(int saleId, string pcName)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("GetByIdAsync", new
                {
                    Id = saleId,
                    PcName = pcName
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });


                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<SaleResponse>(jsonResponse, options);
                }
                else
                    return new SaleResponse
                    {
                        Success = false,
                        Message = $"Error al obtener la venta, el error fue: \n{jsonResponse}",
                    };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    Success = false,
                    Message = $"Error: \n{ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Sale sale)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("UpdateAsync", sale);
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

        internal async Task<SaleResponse> AddAsync(Sale sale)
        {
            try
            {
                SaleViewModel? saleCheck = SaleCache.Instance.FindBySaleNumber(sale.SalePoint, sale.SaleNumber);

                if (saleCheck == null)
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("AddAsync", sale);
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<SaleResponse>(jsonResponse, options);
                }
                else
                    return new SaleResponse
                    {
                        Success = false,
                        Message = "Ese número de venta ya existe",
                    };

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

        internal async Task<SaleResponse> PrintAsync(int saleId)
        {
            try
            {

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("PrintAsync", new
                {
                    Id = saleId,
                });
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SaleResponse>(jsonResponse, options);

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

        internal async Task<SaleResponse> AnullAsync(int saleId)
        {
            try
            {

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("AnullAsync", new
                {
                    Id = saleId,
                    UserName = LoginUserCache.UserName,
                });
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SaleResponse>(jsonResponse, options);

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
