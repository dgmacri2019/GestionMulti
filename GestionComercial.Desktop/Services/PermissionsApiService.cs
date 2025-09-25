using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Services
{
    internal class PermissionsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        private JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public PermissionsApiService()
        {
            _apiService = new ApiService();
            string token = LoginUserCache.AuthToken;
            _httpClient = _apiService.GetHttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LoginUserCache.AuthToken);

        }


        internal async Task<PermissionResponse> GetAllPermissionsAsync(int pageSize = 100)
        {
            List<Permission> allPermissions = [];
            int page = 1;
            bool moreData = true;
            //bool hasPriceList = false, hasStates = false, hasSaleConditions = false, hasIvaConditions = false, hasDocumentTypes = false;
            PermissionResponse permissionResponse = new()
            {
                Success = false
            };
            try
            {
                // Enviar la solicitud al endpoint con parámetros de paginación
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/permissions/GetAllPermissionsAsync", new
                {
                    IsEnabled = true,
                    IsDeleted = false
                });
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await response.Content.ReadAsStreamAsync();

                    IEnumerable<Permission> permissions = await JsonSerializer.DeserializeAsync<IEnumerable<Permission>>(stream, Options);
                    permissionResponse.Permissions = permissions.ToList();
                }
                else
                {
                    permissionResponse.Message = await response.Content.ReadAsStringAsync();
                    return permissionResponse;
                }


                permissionResponse.Success = true;
                //permissionResponse.Permissions = allPermissions;
                return permissionResponse;

            }
            catch (Exception ex)
            {
                permissionResponse.Message = $"Error al obtener permisos, el error fue:\n {ex.Message}";
                return permissionResponse;
            }
        }

        internal async Task<PermissionResponse> GetPermissionByIdAsync(int permissionId)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/permissions/GetPermissionByIdAsync", new
                {
                    Id = permissionId,
                    //IsDeleted = isDeleted,
                    //IsEnabled = isEnabled,
                });


                // Leer el contenido como stream para no cargar todo en memoria
                using Stream? stream = await response.Content.ReadAsStreamAsync();

                return JsonSerializer.Deserialize<PermissionResponse>(stream, Options);

            }
            catch (Exception ex)
            {
                return new PermissionResponse
                {
                    Success = false,
                    Message = $"Error al obtener permiso, el error fue:\n {ex.Message}",
                };
            }
        }

        internal async Task<GeneralResponse> UpdatePermissionAsync(Permission permission)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/permissions/UpdatePermissionAsync", permission);
                string error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error al actualizar permiso, el error fue: \n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Message = $"Error al actualizar permiso, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }

        internal async Task<GeneralResponse> AddPermissionAsync(Permission permission)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/permissions/AddPermissionAsync", permission);
                string error = await response.Content.ReadAsStringAsync();
                return new GeneralResponse
                {
                    Message = $"Error al guardar permiso, el error fue:\n{error}",
                    Success = response.IsSuccessStatusCode,
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse
                {
                    Message = $"Error al guardar permiso, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }

        internal async Task<PermissionResponse> GetUserPermissionsForUserAsync(string userId)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/permissions/GetAllUserPermisionFromUserAsync", new
                {
                    UserId = userId,
                });
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await response.Content.ReadAsStreamAsync();

                    return await JsonSerializer.DeserializeAsync<PermissionResponse>(stream, Options);

                }
                else
                {
                    return new PermissionResponse
                    {
                        Success = false,
                        Message = await response.Content.ReadAsStringAsync(),
                    };
                }

            }
            catch (Exception ex)
            {
                return new PermissionResponse
                {
                    Message = $"Error al guardar permiso, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }

        internal async Task<PermissionResponse> UpdateUserPermissionsAsync(List<UserPermission> userPermissions)
        {
            try
            {
                // Llama al endpoint y deserializa la respuesta

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/permissions/UpdateUserPermissionsAsync", userPermissions);
                string error = await response.Content.ReadAsStringAsync();
                return new PermissionResponse
                {
                    Message = response.IsSuccessStatusCode ? error : $"Error al actualizar los permisos del usuario, el error fue: \n{error}",
                    Success = response.IsSuccessStatusCode,
                };

            }
            catch (Exception ex)
            {
                return new PermissionResponse
                {
                    Message = $"Error al actualizar los permisos del usuario, el error fue: \n{ex.Message}",
                    Success = false,
                };
            }
        }
    }
}
