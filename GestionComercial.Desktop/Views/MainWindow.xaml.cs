using GestionComercial.Desktop.Controls.Articles;
using GestionComercial.Desktop.Controls.Clients;
using GestionComercial.Desktop.Controls.Providers;
using GestionComercial.Desktop.ViewModels;
using System.Windows;

namespace GestionComercial.Desktop.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void Stock_Click(object sender, RoutedEventArgs e)
        {
            // Instanciar la vista de venta de productos.
            var articleView = new ListAticleControlView();

            // Asignar la vista al ContentControl para que se muestre en el área blanca
            MainContent.Content = articleView;
        }

        private void Client_Click(object sender, RoutedEventArgs e)
        {
            // Instanciar la vista de venta de productos.
            var clientView = new ListClientControlView();

            // Asignar la vista al ContentControl para que se muestre en el área blanca
            MainContent.Content = clientView;
        }

        private void Provider_Click(object sender, RoutedEventArgs e)
        {
            // Instanciar la vista de venta de productos.
            var providerView = new ListProviderControlView();

            // Asignar la vista al ContentControl para que se muestre en el área blanca
            MainContent.Content = providerView;
        }
    }
}