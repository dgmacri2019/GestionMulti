using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PcPrinters;
using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PcSalePoints;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters
{
    /// <summary>
    /// Lógica de interacción para PcParametersControlView.xaml
    /// </summary>
    public partial class PcParametersControlView : UserControl
    {
        public PcParametersControlView()
        {
            InitializeComponent();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void BtnSalePonit_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ListPcSalePointControlView();
            lblHeader.Content = "Puntos de Ventas";
            ventana.Actualizado += () =>
            {
                PanelList.Content = null;
                lblHeader.Content = "Parámetros de PC";
            };

            PanelList.Content = ventana;
        }

        private void BtnPrinters_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ListPcPrinterControlView();
            lblHeader.Content = "Impresoras";
            ventana.Actualizado += () =>
            {
                PanelList.Content = null;
                lblHeader.Content = "Parámetros de PC";
            };

            PanelList.Content = ventana;
        }
    }
}
