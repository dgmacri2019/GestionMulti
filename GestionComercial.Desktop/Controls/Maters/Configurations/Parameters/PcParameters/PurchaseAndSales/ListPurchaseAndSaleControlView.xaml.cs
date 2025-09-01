using GestionComercial.Desktop.ViewModels.Parameter;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PurchaseAndSales
{
    /// <summary>
    /// Lógica de interacción para ListPurchaseAndSaleControlView.xaml
    /// </summary>
    public partial class ListPurchaseAndSaleControlView : UserControl
    {
        public event Action Actualizado;

        public ListPurchaseAndSaleControlView()
        {
            InitializeComponent();
            DataContext = new ParameterListViewModel();
            Actualizado?.Invoke();
        }

        private void DgParameter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgParameter.SelectedItem is PurchaseAndSalesListViewModel parameter)
            {
                DgParameter.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                var ventana = new EditPcParameterControlView(parameter.Id);
                ventana.PuntoVentaActualizado += () =>
                {
                    DgParameter.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Actualizado?.Invoke();
        }
    }
}
