using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace GestionComercial.Desktop.Services
{
    internal class MasterClassApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        private JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public MasterClassApiService(string superToken = "")
        {
            _apiService = string.IsNullOrEmpty(superToken) ? new ApiService("api/masterclass/") : new ApiService("api/caches/masterclass/");
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(2000));
        }

        internal async Task<GeneralResponse> AddOrUpdateBillingAsync(BillingViewModel billing)
        {
            try
            {
                using var client = new HttpClient();
                using var form = new MultipartFormDataContent();

                // 1. Archivo
                if (!string.IsNullOrEmpty(billing.CertPath) && File.Exists(billing.CertPath))
                {
                    var fileStream = File.OpenRead(billing.CertPath);
                    var fileContent = new StreamContent(fileStream);
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                    // "file" debe coincidir con el parámetro en la API
                    form.Add(fileContent, "file", Path.GetFileName(billing.CertPath));
                }

                // 2. ViewModel como JSON
                var json = JsonSerializer.Serialize(billing);
                form.Add(new StringContent(json, Encoding.UTF8, "application/json"), "masterclass");


                // Llama al endpoint y deserializa la respuesta
                var response = await _httpClient.PostAsJsonAsync("AddOrUpdateBillingAsync", billing);
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
                    Message = ex.Message,
                };

            }
        }
        internal async Task<GeneralResponse> AddOrUpdateCommerceDataAsync(CommerceData commerceData)
        {
            var response = await _httpClient.PostAsJsonAsync("AddOrUpdateCommerceDataAsync", commerceData);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
        internal async Task<MasterClassResponse> GetAllAsync()
        {
            //bool hasPriceList = false, hasStates = false, hasSaleConditions = false, hasIvaConditions = false, hasDocumentTypes = false;
            MasterClassResponse masterClassResponse = new()
            {
                Success = false
            };
            try
            {
                List<State>? states;
                List<DocumentType>? documentTypes;
                List<IvaCondition>? ivaConditions;
                List<SaleCondition>? saleConditions;
                List<Measure>? measures;
                List<Tax>? taxes;
                // Enviar la solicitud al endpoint Provincias
                HttpResponseMessage responseStates = await _httpClient.PostAsJsonAsync("GetAllStatesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseStates.IsSuccessStatusCode)
                {
                    responseStates.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseStates.Content.ReadAsStreamAsync();

                    states = await JsonSerializer.DeserializeAsync<List<State>>(stream, Options);
                    states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
                    masterClassResponse.States = states;
                }
                else
                {
                    masterClassResponse.Message = await responseStates.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Tipos de Documentos
                HttpResponseMessage responseDocumentType = await _httpClient.PostAsJsonAsync("GetAllDocumentTypesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseDocumentType.IsSuccessStatusCode)
                {
                    responseDocumentType.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseDocumentType.Content.ReadAsStreamAsync();

                    documentTypes = await JsonSerializer.DeserializeAsync<List<DocumentType>>(stream, Options);
                    documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });
                    masterClassResponse.DocumentTypes = documentTypes;
                }
                else
                {
                    masterClassResponse.Message = await responseDocumentType.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Condiciones de IVA
                HttpResponseMessage responseIvaCondition = await _httpClient.PostAsJsonAsync("GetAllIvaConditionsAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseIvaCondition.IsSuccessStatusCode)
                {
                    responseIvaCondition.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseIvaCondition.Content.ReadAsStreamAsync();

                    ivaConditions = await JsonSerializer.DeserializeAsync<List<IvaCondition>>(stream, Options);
                    ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
                    masterClassResponse.IvaConditions = ivaConditions;
                }
                else
                {
                    masterClassResponse.Message = await responseIvaCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Condiciones de Venta
                HttpResponseMessage responseSaleCondition = await _httpClient.PostAsJsonAsync("GetAllSaleConditionsAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseSaleCondition.IsSuccessStatusCode)
                {
                    responseSaleCondition.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseSaleCondition.Content.ReadAsStreamAsync();

                    saleConditions = await JsonSerializer.DeserializeAsync<List<SaleCondition>>(stream, Options);
                    saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
                    masterClassResponse.SaleConditions = saleConditions;
                }
                else
                {
                    masterClassResponse.Message = await responseSaleCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }
                // Enviar la solicitud al endpoint Unidades de medida
                HttpResponseMessage responseMeasure = await _httpClient.PostAsJsonAsync("GetAllMeasuresAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseMeasure.IsSuccessStatusCode)
                {
                    responseMeasure.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseMeasure.Content.ReadAsStreamAsync();

                    measures = await JsonSerializer.DeserializeAsync<List<Measure>>(stream, Options);
                    measures.Add(new Measure { Id = 0, Description = "Seleccione la unidad de medida" });
                    masterClassResponse.Measures = measures;
                }
                else
                {
                    masterClassResponse.Message = await responseSaleCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Tipos de IVA
                HttpResponseMessage responseTax = await _httpClient.PostAsJsonAsync("GetAllTaxesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseTax.IsSuccessStatusCode)
                {
                    responseTax.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseTax.Content.ReadAsStreamAsync();

                    taxes = await JsonSerializer.DeserializeAsync<List<Tax>>(stream, Options);
                    taxes.Add(new Tax { Id = 0, Description = "Seleccione el tipo de IVA" });
                    masterClassResponse.Taxes = taxes;
                }
                else
                {
                    masterClassResponse.Message = await responseSaleCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Datos Comerciales
                HttpResponseMessage responseCommerceData = await _httpClient.PostAsJsonAsync("GetCommerceDataAsync", new
                {

                });
                if (responseCommerceData.IsSuccessStatusCode)
                {
                    responseCommerceData.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseCommerceData.Content.ReadAsStreamAsync();

                    CommerceData? commerceData = await JsonSerializer.DeserializeAsync<CommerceData>(stream, Options);
                    if (commerceData != null && commerceData.Id > 0)
                    {
                        commerceData.States = states;
                        commerceData.IvaConditions = ivaConditions;
                        masterClassResponse.CommerceData = commerceData;
                    }

                }
                else
                {
                    masterClassResponse.Message = await responseCommerceData.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Datos Comerciales
                HttpResponseMessage responseBilling = await _httpClient.PostAsJsonAsync("GetBillingAsync", new
                {

                });
                if (responseBilling.IsSuccessStatusCode)
                {
                    responseBilling.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseBilling.Content.ReadAsStreamAsync();

                    BillingViewModel? billingViewModel = await JsonSerializer.DeserializeAsync<BillingViewModel>(stream, Options);
                    if (billingViewModel.Id == -1)
                        masterClassResponse.BillingViewModel = null;
                    else
                    {
                        masterClassResponse.BillingViewModel = billingViewModel;
                    }
                }
                else
                {
                    masterClassResponse.Message = await responseCommerceData.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }


                masterClassResponse.Success = true;

                return masterClassResponse;

            }
            catch (Exception ex)
            {
                masterClassResponse.Message = $"Error al obtener clases maestras, el error fue:\n {ex.Message}";
                return masterClassResponse;
            }
        }

        #region Category

        internal async Task<CategoryResponse> GetAllCategoriesAsync()
        {
            CategoryResponse result = new()
            {
                Success = false
            };

            try
            {

                // Enviar la solicitud al endpoint Rubros
                HttpResponseMessage responseCategory = await _httpClient.PostAsJsonAsync("GetAllCategoriesAsync", new
                {
                    //IsEnabled = true,
                    //IsDeleted = false
                });
                if (responseCategory.IsSuccessStatusCode)
                {
                    responseCategory.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseCategory.Content.ReadAsStreamAsync();

                    List<CategoryViewModel>? categories = await JsonSerializer.DeserializeAsync<List<CategoryViewModel>>(stream, Options);
                    categories.Add(new CategoryViewModel { Id = 0, Description = "Seleccione el rubro", IsDeleted = true, IsEnabled = true });
                    result.Success = true;
                    result.Categories = categories;
                    return result;
                }
                else
                {
                    result.Message = await responseCategory.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        internal async Task<CategoryResponse> GetCategoryByIdAsync(int id)
        {
            CategoryResponse result = new CategoryResponse
            {
                Success = false
            };

            try
            {

                // Enviar la solicitud al endpoint Rubros
                HttpResponseMessage responseCategory = await _httpClient.PostAsJsonAsync("GetCategoryByIdAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false,
                    Id = id,
                });
                if (responseCategory.IsSuccessStatusCode)
                {
                    responseCategory.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseCategory.Content.ReadAsStreamAsync();

                    CategoryViewModel? category = await JsonSerializer.DeserializeAsync<CategoryViewModel>(stream, Options);
                    result.Success = true;
                    result.Category = category;
                    return result;
                }
                else
                {
                    result.Message = await responseCategory.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        internal async Task<GeneralResponse> AddCategoryAsync(Category category)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("AddCategoryAsync", category);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> UpdateCategoryAsync(Category category)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("UpdateCategoryAsync", category);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        #endregion


        #region PriceList
        internal async Task<PriceListResponse> GetAllPriceListAsync()
        {
            PriceListResponse result = new()
            {
                Success = false
            };

            try
            {

                // Enviar la solicitud al endpoint Rubros
                HttpResponseMessage responsePriceList = await _httpClient.PostAsJsonAsync("GetAllPriceListAsync", new
                {
                    //IsEnabled = true,
                    //IsDeleted = false
                });
                if (responsePriceList.IsSuccessStatusCode)
                {
                    responsePriceList.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responsePriceList.Content.ReadAsStreamAsync();

                    List<PriceListViewModel>? priceLists = await JsonSerializer.DeserializeAsync<List<PriceListViewModel>>(stream, Options);
                    priceLists.Add(new PriceListViewModel { Id = 0, Description = "Seleccione la lista de precios", IsDeleted = true, IsEnabled = true });
                    result.Success = true;
                    result.PriceListViewModels = priceLists;
                    return result;
                }
                else
                {
                    result.Message = await responsePriceList.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        internal async Task<PriceListResponse> GetPriceListByIdAsync(int id)
        {
            PriceListResponse result = new()
            {
                Success = false
            };

            try
            {

                // Enviar la solicitud al endpoint Rubros
                HttpResponseMessage responsePriceList = await _httpClient.PostAsJsonAsync("GetPriceListByIdAsync", new
                {
                    //IsEnabled = true,
                    //IsDeleted = false,
                    Id = id,
                });
                if (responsePriceList.IsSuccessStatusCode)
                {
                    responsePriceList.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responsePriceList.Content.ReadAsStreamAsync();

                    PriceListViewModel? priceList = await JsonSerializer.DeserializeAsync<PriceListViewModel>(stream, Options);
                    result.Success = true;
                    result.PriceListViewModel = priceList;
                    return result;
                }
                else
                {
                    result.Message = await responsePriceList.Content.ReadAsStringAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
        }

        internal async Task<GeneralResponse> AddPriceListAsync(PriceList priceList)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("AddPriceListAsync", priceList);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }

        internal async Task<GeneralResponse> UpdatePriceListAsync(PriceList priceList)
        {
            // Llama al endpoint y deserializa la respuesta

            var response = await _httpClient.PostAsJsonAsync("UpdatePriceListAsync", priceList);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
               
        #endregion

               
    }
}
