using GestionComercial.Desktop.ViewModels.Bank;
using GestionComercial.Domain.DTOs.Banks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Banks
{
    /// <summary>
    /// Lógica de interacción para ListBankParameterControlView.xaml
    /// </summary>
    public partial class ListBankParameterControlView : UserControl
    {
        private bool Enabled = true;
        private bool Deleted = false;

        public ListBankParameterControlView()
        {
            InitializeComponent();
            DataContext = new BankParameterListViewModel(string.Empty, Enabled, Deleted);
        }






        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgBankParameter.SelectedItem is BankParameterViewModel bankParameterViewModel)
            {
                DgBankParameter.Visibility = Visibility.Hidden;
                DgBankParameter.DataContext = null;
                PanelEdicion.Visibility = Visibility.Visible;
                btnAddBank.Visibility = Visibility.Hidden;
                lblHeader.Content = "Editar Parametro de acreditación bancaria";
                var ventana = new EditBankParameterControlView(bankParameterViewModel.Id);
                ventana.ParametroActualizado += () =>
                {
                    DgBankParameter.DataContext = new BankParameterListViewModel(string.Empty, Enabled, Deleted);
                    DgBankParameter.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    btnAddBank.Visibility = Visibility.Visible;
                    lblHeader.Content = "Parametros de acreditaciones bancarias";
                };
                PanelEdicion.Content = ventana;

            }
        }

        private void btnAddParameter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
