using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using System.Windows;

namespace GestionComercial.Desktop.Views
{
    public partial class LoginWindow : Window
    {
        private readonly AuthApiService _authService = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            string token = await _authService.LoginAsync(username, password);

            if (!string.IsNullOrWhiteSpace(token))
            {
                App.UserName = TokenHelper.GetUsername(token);
                App.UserRole = TokenHelper.GetRole(token);
                App.AuthToken = TokenHelper.ExtractTokenValue(token);

                // Abrir ventana principal y pasar el token
                MainWindow main = new();
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Credenciales inválidas", "Login", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}