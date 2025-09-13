using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
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

        public MasterClassApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(2000));
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

                // Enviar la solicitud al endpoint Lista De Precios
                HttpResponseMessage responsePriceList = await _httpClient.PostAsJsonAsync("api/pricelists/GetAllAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responsePriceList.IsSuccessStatusCode)
                {
                    responsePriceList.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responsePriceList.Content.ReadAsStreamAsync();

                    List<PriceList>? priceLists = await JsonSerializer.DeserializeAsync<List<PriceList>>(stream, Options);
                    priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });
                    masterClassResponse.PriceLists = priceLists;
                }
                else
                {
                    masterClassResponse.Message = await responsePriceList.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Provincias
                HttpResponseMessage responseStates = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllStatesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseStates.IsSuccessStatusCode)
                {
                    responseStates.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseStates.Content.ReadAsStreamAsync();

                    List<State>? states = await JsonSerializer.DeserializeAsync<List<State>>(stream, Options);
                    states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
                    masterClassResponse.States = states;
                }
                else
                {
                    masterClassResponse.Message = await responseStates.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Tipos de Documentos
                HttpResponseMessage responseDocumentType = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllDocumentTypesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseDocumentType.IsSuccessStatusCode)
                {
                    responseDocumentType.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseDocumentType.Content.ReadAsStreamAsync();

                    List<DocumentType>? documentTypes = await JsonSerializer.DeserializeAsync<List<DocumentType>>(stream, Options);
                    documentTypes.Add(new DocumentType { Id = 0, Description = "Seleccione el tipo de documento" });
                    masterClassResponse.DocumentTypes = documentTypes;
                }
                else
                {
                    masterClassResponse.Message = await responseDocumentType.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Condiciones de IVA
                HttpResponseMessage responseIvaCondition = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllIvaConditionsAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseIvaCondition.IsSuccessStatusCode)
                {
                    responseIvaCondition.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseIvaCondition.Content.ReadAsStreamAsync();

                    List<IvaCondition>? ivaConditions = await JsonSerializer.DeserializeAsync<List<IvaCondition>>(stream, Options);
                    ivaConditions.Add(new IvaCondition { Id = 0, Description = "Seleccione la condición de IVA" });
                    masterClassResponse.IvaConditions = ivaConditions;
                }
                else
                {
                    masterClassResponse.Message = await responseIvaCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Condiciones de Venta
                HttpResponseMessage responseSaleCondition = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllSaleConditionsAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseSaleCondition.IsSuccessStatusCode)
                {
                    responseSaleCondition.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseSaleCondition.Content.ReadAsStreamAsync();

                    List<SaleCondition>? saleConditions = await JsonSerializer.DeserializeAsync<List<SaleCondition>>(stream, Options);
                    saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
                    masterClassResponse.SaleConditions = saleConditions;
                }
                else
                {
                    masterClassResponse.Message = await responseSaleCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }
                // Enviar la solicitud al endpoint Unidades de medida
                HttpResponseMessage responseMeasure = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllMeasuresAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseMeasure.IsSuccessStatusCode)
                {
                    responseMeasure.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseMeasure.Content.ReadAsStreamAsync();

                    List<Measure>? measures = await JsonSerializer.DeserializeAsync<List<Measure>>(stream, Options);
                    measures.Add(new Measure { Id = 0, Description = "Seleccione la unidad de medida" });
                    masterClassResponse.Measures = measures;
                }
                else
                {
                    masterClassResponse.Message = await responseSaleCondition.Content.ReadAsStringAsync();
                    return masterClassResponse;
                }

                // Enviar la solicitud al endpoint Tipos de IVA
                HttpResponseMessage responseTax = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllTaxesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseTax.IsSuccessStatusCode)
                {
                    responseTax.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseTax.Content.ReadAsStreamAsync();

                    List<Tax>? taxes = await JsonSerializer.DeserializeAsync<List<Tax>>(stream, Options);
                    taxes.Add(new Tax { Id = 0, Description = "Seleccione el tipo de IVA" });
                    masterClassResponse.Taxes = taxes;
                }
                else
                {
                    masterClassResponse.Message = await responseSaleCondition.Content.ReadAsStringAsync();
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

        internal async Task<CategoryResponse> GetAllCategoriesAsync()
        {
            CategoryResponse result = new CategoryResponse
            {
                Success = false
            };

            try
            {

                // Enviar la solicitud al endpoint Rubros
                HttpResponseMessage responseCategory = await _httpClient.PostAsJsonAsync("api/masterclass/GetAllCategoriesAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (responseCategory.IsSuccessStatusCode)
                {
                    responseCategory.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await responseCategory.Content.ReadAsStreamAsync();

                    List<CategoryViewModel>? categories = await JsonSerializer.DeserializeAsync<List<CategoryViewModel>>(stream, Options);
                    categories.Add(new CategoryViewModel { Id = 0, Description = "Seleccione el rubro", IsDeleted = true, IsEnabled = false });
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
                HttpResponseMessage responseCategory = await _httpClient.PostAsJsonAsync("api/masterclass/GetCategoryByIdAsync", new
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

            var response = await _httpClient.PostAsJsonAsync("api/masterclass/AddCategoryAsync", category);
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

            var response = await _httpClient.PostAsJsonAsync("api/masterclass/UpdateCategoryAsync", category);
            var error = await response.Content.ReadAsStringAsync();
            return new GeneralResponse
            {
                Message = $"Error: {response.StatusCode}\n{error}",
                Success = response.IsSuccessStatusCode,
            };
        }
    }
}
