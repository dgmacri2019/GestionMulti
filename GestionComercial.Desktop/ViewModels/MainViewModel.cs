using GestionComercial.Desktop.Utils;
using GestionComercial.Desktop.Views;
using GestionComercial.Domain.Cache;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace GestionComercial.Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ICommand LogoutCommand { get; }

        private string _horaActual;
        public string HoraActual
        {
            get => _horaActual;
            set => SetProperty(ref _horaActual, value);
        }

        private string _usuario;
        public string Usuario
        {
            get => _usuario;
            set => SetProperty(ref _usuario, value);
        }

        private string _rol;
        public string Rol
        {
            get => _rol;
            set => SetProperty(ref _rol, value);
        }

        private readonly DispatcherTimer _timer;

        public MainViewModel()
        {
            LogoutCommand = new RelayCommand(Logout);
            HoraActual = DateTime.Now.ToString("HH:mm:ss");
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += (s, e) => HoraActual = DateTime.Now.ToString("HH:mm:ss");
            _timer.Start();

            Usuario = LoginUserCache.UserName ?? "Invitado";
            Rol = LoginUserCache.UserRole ?? "Rol Sin Definir";
        }


        private void Logout()
        {
            // Limpiar sesión
            LoginUserCache.AuthToken = string.Empty;
            LoginUserCache.UserName = string.Empty;
            LoginUserCache.UserRole = string.Empty;
            LoginUserCache.Password = string.Empty;

            LoginWindow loginView = new LoginWindow();
            loginView.Show();
            // Importante: cerrar la ventana actual correctamente
            foreach (Window window in Application.Current.Windows)
            {
                if (window is MainWindow)
                {
                    window.Close();
                    break;
                }
            }

            // Establecer login como nueva MainWindow si querés seguir con el flujo
            Application.Current.MainWindow = loginView;
        }
    }

}
