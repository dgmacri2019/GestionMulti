using System.Net.Http;
using System.Net.Http.Headers;

namespace GestionComercial.Desktop.Helpers
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService()
        {
            var config = ConfigurationHelper.GetConfiguration();
            string baseUrl = config["ApiSettings:BaseUrl"];

            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromMilliseconds(10000),                
                /*, MaxResponseContentBufferSize = 100_000_000*/
            };
        }

        public HttpClient GetHttpClient() => _client;
    }
}
