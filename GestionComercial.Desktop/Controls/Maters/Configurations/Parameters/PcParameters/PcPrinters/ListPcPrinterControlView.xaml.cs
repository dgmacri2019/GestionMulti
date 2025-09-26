using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PcSalePoints;
using GestionComercial.Desktop.ViewModels.Parameter;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PcPrinters
{
    /// <summary>
    /// Lógica de interacción para ListPcPrinterControlView.xaml
    /// </summary>
    public partial class ListPcPrinterControlView : UserControl
    {
        public event Action Actualizado;

        public ListPcPrinterControlView()
        {
            InitializeComponent();
            DataContext = null;
            Actualizado?.Invoke();
        }

        private void DgPcPrinterParameter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgPcPrinterParameter.SelectedItem is PcPrinterParametersListViewModel parameter)
            {
                DgPcPrinterParameter.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                var ventana = new EditPcPrinterControlView(parameter.Id);
                ventana.ImpresorasActualizadas += () =>
                {
                    DgPcPrinterParameter.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Actualizado?.Invoke();
        }
    }
}
