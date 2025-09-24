using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Response;
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
            btnLogin.IsEnabled = false;
            lblError.Text = string.Empty;
            string username = txtUsername.Text;
            string password = txtPassword.Password;
            string token = string.Empty;

            LoginResponse resultLogin = await _authService.LoginAsync(username, password);
            if (resultLogin.Success)
            {
                token = resultLogin.Token;
                if (!string.IsNullOrWhiteSpace(token))
                {
                    LoginUserCache.UserName = TokenHelper.GetUsername(token);
                    LoginUserCache.UserRole = TokenHelper.GetRole(token);
                    LoginUserCache.AuthToken = resultLogin.Token;
                    LoginUserCache.UserId = resultLogin.UserId;
                    LoginUserCache.Permisions = resultLogin.Permissions;

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
            else
            {
                lblError.Text = resultLogin.Message;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }

        private void txtUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(txtUsername.Text))
                txtPassword.Focus();
        }
    }
}