using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Json;

namespace GestionComercial.Desktop.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService()
        {

            _httpClient = new ApiService().GetHttpClient();
            _httpClient.Timeout.Add(new TimeSpan(100));
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync("api/auth/LoginAsync", new { username, password });
                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    return new LoginResponse
                    {
                        Success = true,
                        Token = token.Trim('"'), // Elimina comillas si viene como string
                    };
                }
                var result = await response.Content.ReadAsStringAsync();
                return new LoginResponse { Success = false, Message = result };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
