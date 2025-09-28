using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.GeneralParameters.EmailParameters;
using GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.GeneralParameters.GeneralParameters;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.GeneralParameters
{
    /// <summary>
    /// Lógica de interacción para GeneralParametersControlView.xaml
    /// </summary>
    public partial class GeneralParametersControlView : UserControl
    {
        public GeneralParametersControlView()
        {
            InitializeComponent();
        }

        private void BtnGeneralParameters_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new EditGeneralParameterControlView();
            lblHeader.Content = "Parámetros Generales";
            ventana.ParametroGeneralActualizado += () =>
            {
                PanelList.Content = null;
                lblHeader.Content = "Parámetros Generales de la empresa";
            };

            PanelList.Content = ventana;
        }

        private void BtnEmailParameters_Click(object sender, RoutedEventArgs e)
        {
            var ventana = new EditEmailGeneralParameterControlView();
            lblHeader.Content = "Parámetros de Email";
            ventana.ParametroEmailActualizado += () =>
            {
                PanelList.Content = null;
                lblHeader.Content = "Parámetros Generales de la empresa";
            };

            PanelList.Content = ventana;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
