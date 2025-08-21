using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Response;
using System.Net.Http;
using System.Net.Http.Json;

namespace GestionComercial.Desktop.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _client;

        public AuthApiService()
        {

            _client = new ApiService().GetHttpClient();
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {

                var response = await _client.PostAsJsonAsync("api/auth/LoginAsync", new { username, password });

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    return new LoginResponse
                    {
                        Success = true,
                        Token = token.Trim('"'), // Elimina comillas si viene como string
                    };
                }

                return new LoginResponse { Success = false, Message = response.RequestMessage.ToString() };
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
