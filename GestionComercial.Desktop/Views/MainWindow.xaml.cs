using GestionComercial.Desktop.Controls;
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
            // Puede ser un UserControl o una Page; en este ejemplo, se llama ProductSaleView.
            var productSaleView = new ProductSaleControlView();

            // Asignar la vista al ContentControl para que se muestre en el área blanca
            MainContent.Content = productSaleView;
        }
    }
}