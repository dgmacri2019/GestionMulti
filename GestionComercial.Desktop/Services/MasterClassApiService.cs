using GestionComercial.Desktop.Helpers;
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
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

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

                    List<PriceList>? priceLists = await JsonSerializer.DeserializeAsync<List<PriceList>>(stream, options);
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

                    List<State>? states = await JsonSerializer.DeserializeAsync<List<State>>(stream, options);
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

                    List<DocumentType>? documentTypes = await JsonSerializer.DeserializeAsync<List<DocumentType>>(stream, options);
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

                    List<IvaCondition>? ivaConditions = await JsonSerializer.DeserializeAsync<List<IvaCondition>>(stream, options);
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

                    List<SaleCondition>? saleConditions = await JsonSerializer.DeserializeAsync<List<SaleCondition>>(stream, options);
                    masterClassResponse.SaleConditions = saleConditions;
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


    }
}
