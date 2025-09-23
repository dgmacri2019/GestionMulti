using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GestionComercial.Desktop.Services
{
    internal class ClientsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ClientsApiService()
        {
            _apiService = new ApiService();
            string token = LoginUserCache.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);
        }


        internal async Task<ClientResponse> GetAllAsync(int pageSize = 100)
        {
            List<ClientViewModel> allClients = [];
            int page = 1;
            bool moreData = true;
            //bool hasPriceList = false, hasStates = false, hasSaleConditions = false, hasIvaConditions = false, hasDocumentTypes = false;
            ClientResponse clientResponse = new()
            {
                Success = false
            };
            try
            {
                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                while (moreData)
                {
                    // Enviar la solicitud al endpoint con parámetros de paginación
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/clients/GetAllAsync", new
                    {
                        Page = page,
                        PageSize = pageSize
                    });
                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // Leer el contenido como stream para no cargar todo en memoria
                        using Stream? stream = await response.Content.ReadAsStreamAsync();

                        ClientResponse? result = await JsonSerializer.DeserializeAsync<ClientResponse>(stream, options);
                        if (result.Success)
                        {                            
                            if (result.ClientViewModels == null || result.ClientViewModels.Count() == 0)
                            {
                                moreData = false; // no quedan más datos
                            }
                            else
                            {
                                allClients.AddRange(result.ClientViewModels);
                                page++; // siguiente página
                            }
                        }
                    }
                    else
                    {
                        clientResponse.Message = await response.Content.ReadAsStringAsync();
                        return clientResponse;
                    }
                }

                clientResponse.Success = true;
                clientResponse.ClientViewModels = allClients;
                return clientResponse;

            }
            catch (Exception ex)
            {
                clientResponse.Message = $"Error al obtener clientes, el error fue:\n {ex.Message}";
                return clientResponse;
            }
        }

        internal async Task<ClientResponse> GetByIdAsync(int clientId)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/clients/GetByIdAsync", new
                {
                    Id = clientId,
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });

                JsonSerializerOptions options = new()
                {
                    PropertyNameCaseInsensitive = true
                };
                string jsonResponse = await response.Content.ReadAsStringAsync();

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
                        Message = $"Error al obtener cliente, el error fue:\n{jsonResponse}",
                    };
            }
            catch (Exception ex)
            {
                return new ClientResponse
                {
                    Success = false,
                    Message = $"Error al obtener cliente, el error fue:\n {ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(Client client)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/clients/UpdateAsync", client);
                string error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error al actualizar cliente, el error fue: \n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Message = $"Error al actualizar cliente, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }

        internal async Task<GeneralResponse> AddAsync(Client client)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/clients/AddAsync", client);
                string error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error al guardar cliente, el error fue:\n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Message = $"Error al guardar cliente, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }
    }
}
