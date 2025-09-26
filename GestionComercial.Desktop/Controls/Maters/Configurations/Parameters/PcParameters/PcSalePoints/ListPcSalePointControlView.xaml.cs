using GestionComercial.Desktop.ViewModels.Parameter;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PcSalePoints
{
    /// <summary>
    /// Lógica de interacción para ListPcSalePointControlView.xaml
    /// </summary>
    public partial class ListPcSalePointControlView : UserControl
    {
        public event Action Actualizado;

        public ListPcSalePointControlView()
        {
            InitializeComponent();
            DataContext = new PcParameterListViewModel();
            Actualizado?.Invoke();
        }

        private void DgParameter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgParameter.SelectedItem is PcSalePointsListViewModel parameter)
            {
                DgParameter.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                var ventana = new EditPcSalePointControlView(parameter.Id);
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
