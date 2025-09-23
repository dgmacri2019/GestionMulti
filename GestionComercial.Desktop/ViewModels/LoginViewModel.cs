using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Utils;
using GestionComercial.Desktop.Views;
using GestionComercial.Domain.Cache;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Input;

namespace GestionComercial.Desktop.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(async () => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(LoginUserCache.Password))
            {
                MessageBox.Show("Ingrese usuario y contraseña.");
                return;
            }
            var config = ConfigurationHelper.GetConfiguration();
            string baseUrl = config["ApiSettings:BaseUrl"];
            using HttpClient client = new() { BaseAddress = new Uri(baseUrl) };
            var loginData = new { username = Username, password = LoginUserCache.Password };
            string jsonData = JsonSerializer.Serialize(loginData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("api/auth/login", content);

            if (response.IsSuccessStatusCode)
            {
                string token = await response.Content.ReadAsStringAsync();
                LoginUserCache.AuthToken = token; // Guardamos el token globalmente
                MessageBox.Show("Inicio de sesión exitoso.");

                // Abrir ventana principal
                MainWindow mainView = new();
                mainView.Show();

                // Cerrar login
                Application.Current.Windows[0]?.Close();
            }
            else
            {
                MessageBox.Show("Error en el inicio de sesión.");
            }
        }
    }
}
