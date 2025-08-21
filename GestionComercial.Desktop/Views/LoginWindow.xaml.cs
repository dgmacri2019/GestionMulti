using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using System.Windows;
using System.Windows.Input;

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
            await LoginAsync();
        }


        private async Task LoginAsync()
        {
            lblError.Text = string.Empty;
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            btnLogin.IsEnabled = false;
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
                lblError.Text = "Credenciales inválidas";
                btnLogin.IsEnabled = true;
            }
        }

        private async void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {                
                await LoginAsync();
            }
        }
    }
}