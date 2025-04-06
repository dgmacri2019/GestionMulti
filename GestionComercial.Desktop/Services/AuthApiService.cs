using GestionComercial.Desktop.Helpers;
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

        public async Task<string> LoginAsync(string username, string password)
        {
            var response = await _client.PostAsJsonAsync("api/auth/login", new { username, password });

            if (response.IsSuccessStatusCode)
            {
                var token = await response.Content.ReadAsStringAsync();
                return token.Trim('"'); // Elimina comillas si viene como string
            }

            return null;
        }
    }
}
