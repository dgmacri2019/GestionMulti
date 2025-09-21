using GestionComercial.Desktop.ViewModels.Sale;
using GestionComercial.Desktop.Views.Sales;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Sales
{
    /// <summary>
    /// Lógica de interacción para ListSaleControlView.xaml
    /// </summary>
    public partial class ListSaleControlView : UserControl
    {
        public ListSaleControlView()
        {
            InitializeComponent();
            DataContext = new SaleListViewModel();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saleAddWindow = new SaleAddWindow(0) { Owner = Window.GetWindow(this) };
                saleAddWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
    }
}
