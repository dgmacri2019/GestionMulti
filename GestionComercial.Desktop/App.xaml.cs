using GestionComercial.Desktop.Views;
using System.Globalization;
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
        {// Seteo la cultura a Argentina
         // Seteo cultura a Argentina
            var culture = new CultureInfo("es-AR");

            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    System.Windows.Markup.XmlLanguage.GetLanguage(culture.IetfLanguageTag)));
            base.OnStartup(e);
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }

}
