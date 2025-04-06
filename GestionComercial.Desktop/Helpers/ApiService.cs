using System.Net.Http;

namespace GestionComercial.Desktop.Helpers
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            var config = ConfigurationHelper.GetConfiguration();
            string baseUrl = config["ApiSettings:BaseUrl"];

            _client = new HttpClient { BaseAddress = new Uri(baseUrl) };
        }

        public HttpClient GetHttpClient() => _client;
    }
}
