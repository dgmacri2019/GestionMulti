using GestionComercial.Desktop.Controls.Articles;
using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PurchaseAndSales;
using GestionComercial.Domain.Entities.Stock;
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

        private void BtnPurchaseAndSalePonit_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new ListPurchaseAndSaleControlView();
            lblHeader.Content = "Parámetros de Compras y Ventas";
            ventana.Actualizado += () =>
            {
                PanelList.Content = null;
                lblHeader.Content = "Parámetros de PC";
            };

            PanelList.Content = ventana;
        }

        private void BtnPrinters_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
