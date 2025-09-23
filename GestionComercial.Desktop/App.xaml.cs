using GestionComercial.Desktop.Views;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.Windows;

namespace GestionComercial.Desktop
{
    /// <summary>
    /// Interaction logic for LoginUserCache.xaml
    /// </summary>
    public partial class App : Application
    {        
        public static IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            // Seteo la cultura a Argentina
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
