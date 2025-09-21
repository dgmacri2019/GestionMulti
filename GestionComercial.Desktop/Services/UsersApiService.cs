using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace GestionComercial.Desktop.Services
{
    internal class UsersApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        private JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public UsersApiService()
        {
            _apiService = new ApiService();
            string token = App.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthToken);
        }

        internal async Task<UserResponse> GetAllAsync(int pageSize = 100)
        {
            List<UserViewModel> allUsers = [];
            int page = 1;
            bool moreData = true;
            //bool hasPriceList = false, hasStates = false, hasSaleConditions = false, hasIvaConditions = false, hasDocumentTypes = false;
            UserResponse userResponse = new()
            {
                Success = false
            };
            try
            {
                // Llama al endpoint y deserializa la respuesta
                while (moreData)
                {
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users/GetAllAsync", new
                    {
                        Page = page,
                        PageSize = pageSize
                    });


                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();

                        // Leer el contenido como stream para no cargar todo en memoria
                        using Stream? stream = await response.Content.ReadAsStreamAsync();

                        UserResponse? result = await JsonSerializer.DeserializeAsync<UserResponse>(stream, Options);
                        if (result.Success)
                        {
                            if (result.UserViewModels == null || result.UserViewModels.Count() == 0)
                            {
                                moreData = false; // no quedan más datos
                            }
                            else
                            {
                                allUsers.AddRange(result.UserViewModels);
                                page++; // siguiente página
                            }
                        }
                    }
                    else
                    {
                        userResponse.Message = await response.Content.ReadAsStringAsync();
                        return userResponse;
                    }
                }
                userResponse.Success = true;
                userResponse.UserViewModels = allUsers;
                return userResponse;

            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        internal async Task<UserResponse> GetByIdAsync(string userId)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users/GetByIdAsync", new
                {
                    Id = userId,
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });


                string jsonResponse = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return new UserResponse
                    {
                        UserViewModel = JsonSerializer.Deserialize<UserViewModel>(jsonResponse, Options),
                        Success = true,
                    };
                else
                    return new UserResponse
                    {
                        Success = false,
                        Message = $"Error al obtener usuario, el error fue:\n{jsonResponse}",
                    };
            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = $"Error al obtener usuario, el error fue:\n {ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdateAsync(User user)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users/UpdateAsync", user);
                string error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error al actualizar usuario, el error fue: \n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Message = $"Error al actualizar usuario, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }

        internal async Task<GeneralResponse> AddAsync(User user)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/users/AddAsync", user);
                string error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error al guardar usuario, el error fue:\n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Message = $"Error al guardar usuario, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }



    }
}
