using System.Net.Http;
using System.Net.Http.Headers;

namespace GestionComercial.Desktop.Helpers
{
    public class ApiService
    {
        private readonly HttpClient _client;

        public ApiService(string controllerName)
        {
            var config = ConfigurationHelper.GetConfiguration();
            string baseUrl = string.Format("{0}{1}", config["ApiSettings:BaseUrl"], controllerName);

            _client = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                //Timeout = TimeSpan.FromMilliseconds(10000),                
                /*, MaxResponseContentBufferSize = 100_000_000*/
            };
        }

        public HttpClient GetHttpClient() => _client;
    }
}
