using GestionComercial.Desktop.Views;
using System.Configuration;
using System.Data;
using System.Windows;

namespace GestionComercial.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string AuthToken { get; set; }
        public static string Password { get; set; }
        public static string UserName { get; set; }
        public static string UserRole { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }

}
