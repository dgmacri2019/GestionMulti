using GestionComercial.Desktop.Helpers;
using GestionComercial.Domain.Response;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace GestionComercial.Desktop.Services
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true
        };
        public AuthApiService()
        {

            _httpClient = new ApiService().GetHttpClient();
            _httpClient.Timeout.Add(TimeSpan.FromMilliseconds(200));
        }

        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync("api/auth/LoginAsync", new { username, password });
                if (response.IsSuccessStatusCode)
                {
                    response.EnsureSuccessStatusCode();

                    //var jsonResponse = await response.Content.ReadAsStringAsync();
                    //return new LoginResponse
                    //{
                    //    Success = true,
                    //    Token = jsonResponse.Trim('"'), // Elimina comillas si viene como string
                    //};

                    // Leer el contenido como stream para no cargar todo en memoria
                    using Stream? stream = await response.Content.ReadAsStreamAsync();
                    LoginResponse? resultLogin = JsonSerializer.Deserialize<LoginResponse>(stream, Options);

                    if (!resultLogin.Success)
                        return resultLogin;


                    if (resultLogin.Success && resultLogin.Token == null)
                        return new LoginResponse
                        {
                            Success = false,
                            Message = "Usuario o contraseña inválidos",
                        };

                    return resultLogin;

                    return new LoginResponse
                    {
                        Success = true,
                        UserId = resultLogin.UserId,
                        Token = resultLogin.Token.Trim('"'), // Elimina comillas si viene como string
                        //Token = resultLogin.Token, // Elimina comillas si viene como string
                        Permissions = resultLogin.Permissions,
                    };
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.ServiceUnavailable)
                        return new LoginResponse
                        {
                            Success = false,
                            Message = "El servidor no se ecnuentra disponible"
                        };
                    return new LoginResponse
                    {
                        Success = false,
                        Message = $"Error: código: {response.StatusCode}\n {response.ReasonPhrase}",
                    };
                }
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
