using GestionComercial.Desktop.Utils;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using GestionComercial.Desktop.Views;

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

            Usuario = App.UserName ?? "Invitado";
            Rol = App.UserRole ?? "Rol Sin Definir";
        }


        private void Logout()
        {
            // Limpiar sesión
            App.AuthToken = string.Empty;
            App.UserName = string.Empty;
            App.UserRole = string.Empty;
            App.Password = string.Empty;

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
