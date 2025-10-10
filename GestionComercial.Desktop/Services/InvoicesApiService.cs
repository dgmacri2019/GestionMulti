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
using System.Xaml.Schema;

namespace GestionComercial.Desktop.Services
{
    internal class InvoicesApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;
        private readonly JsonSerializerOptions options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public InvoicesApiService(string superToken = "")
        {
            _apiService = string.IsNullOrEmpty(superToken) ? new ApiService("api/invoices/") : new ApiService("api/caches/invoices/");
            string token = LoginUserCache.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }


        internal async Task<InvoiceResponse> GetAllAsync(int pageSize = 100)
        {
            // Llama al endpoint y deserializa la respuesta
            List<Invoice> allInvoices = [];
            int page = 1;
            bool moreData = true;
            InvoiceResponse invoiceResponse = new()
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


                        InvoiceResponse? result = JsonSerializer.Deserialize<InvoiceResponse>(stream, options);
                        if (result.Success)
                        {
                            if (result.Invoices == null || result.Invoices.Count() == 0)
                            {
                                moreData = false; // no quedan más datos
                            }
                            else
                            {
                                allInvoices.AddRange(result.Invoices);
                                page++; // siguiente página
                            }
                        }
                    }
                    else
                    {
                        invoiceResponse.Message = await response.Content.ReadAsStringAsync();
                        return invoiceResponse;
                    }
                }
                invoiceResponse.Success = true;
                invoiceResponse.Invoices = allInvoices;
                return invoiceResponse;

            }
            catch (Exception ex)
            {
                invoiceResponse.Message = $"Error al obtener facturas, el error fue:\n {ex.Message}";
                return invoiceResponse;
            }
        }

        internal async Task<InvoiceResponse> GetAllBySalePointAsync(int salePoint, int pageSize = 100)
        {
            // Llama al endpoint y deserializa la respuesta
            List<Invoice> allInvoices = [];
            int page = 1;
            bool moreData = true;
            InvoiceResponse invocieResponse = new()
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


                        InvoiceResponse? result = JsonSerializer.Deserialize<InvoiceResponse>(stream, options);
                        if (result.Success)
                        {
                            if (result.Invoices == null || result.Invoices.Count() == 0)
                            {
                                moreData = false; // no quedan más datos                                
                            }
                            else
                            {
                                allInvoices.AddRange(result.Invoices);
                                page++; // siguiente página
                            }
                        }
                    }
                    else
                    {
                        invocieResponse.Message = await response.Content.ReadAsStringAsync();
                        return invocieResponse;
                    }
                }
                invocieResponse.Success = true;
                invocieResponse.Invoices = allInvoices;
                return invocieResponse;
            }
            catch (Exception ex)
            {
                invocieResponse.Message = $"Error al obtener facturas, el error fue:\n {ex.Message}";
                return invocieResponse;
            }
        }

        internal async Task<InvoiceResponse> GetByIdAsync(int saleId, string pcName)
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
                    return JsonSerializer.Deserialize<InvoiceResponse>(jsonResponse, options);
                }
                else
                    return new InvoiceResponse
                    {
                        Success = false,
                        Message = $"Error al obtener la factura, el error fue: \n{jsonResponse}",
                    };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = $"Error: \n{ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Invoice invoice)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                var response = await _httpClient.PostAsJsonAsync("UpdateAsync", invoice);
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

        internal async Task<InvoiceResponse> AddAsync(int saleId)
        {
            try
            {
                HttpResponseMessage responseAddInvoice = await _httpClient.PostAsJsonAsync("AddAsync", new
                {
                    Id = saleId,
                });

                string jsonResponseAddInvoice = await responseAddInvoice.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InvoiceResponse>(jsonResponseAddInvoice, options);
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        internal async Task<InvoiceResponse> AnullAsync(int saleId, int salePoint)
        {
            try
            {
                HttpResponseMessage responseAddInvoice = await _httpClient.PostAsJsonAsync("AnullAsync", new
                {
                    Id = saleId,
                    UserName = LoginUserCache.UserName,
                    SalePoint = salePoint,
                });

                string jsonResponseAddInvoice = await responseAddInvoice.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<InvoiceResponse>(jsonResponseAddInvoice, options);
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}

