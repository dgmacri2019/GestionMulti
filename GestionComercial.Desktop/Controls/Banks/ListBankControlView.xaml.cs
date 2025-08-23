using GestionComercial.Desktop.ViewModels.Bank;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Banks
{
    /// <summary>
    /// Lógica de interacción para ListBankControlView.xaml
    /// </summary>
    public partial class ListBankControlView : UserControl
    {
        public ListBankControlView()
        {
            InitializeComponent();
            DataContext = new BoxAndBankListViewModel();
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgBoxAndBanks.SelectedItem is BankAndBoxViewModel bankAndBoxViewModel)
            {
                DgBoxAndBanks.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                btnAddBank.Visibility = Visibility.Hidden;
                btnAddBox.Visibility = Visibility.Hidden;
                if (bankAndBoxViewModel.IsBank)
                {
                    lblHeader.Content = "Editar Banco";
                    var ventana = new EditBankControlView(bankAndBoxViewModel.Id);
                    ventana.BancoActualizado += () =>
                    {
                        DgBoxAndBanks.Visibility = Visibility.Visible;
                        PanelEdicion.Content = null;
                        PanelEdicion.Visibility = Visibility.Hidden;
                        btnAddBank.Visibility = Visibility.Visible;
                        btnAddBox.Visibility = Visibility.Visible;
                        lblHeader.Content = "Cajas y Bancos";
                    };
                    PanelEdicion.Content = ventana;
                }
                else
                {
                    lblHeader.Content = "Editar Caja";
                    var ventana = new EditBoxControlView(bankAndBoxViewModel.Id);
                    ventana.CajaActualizada += () =>
                    {
                        DgBoxAndBanks.Visibility = Visibility.Visible;
                        PanelEdicion.Content = null;
                        PanelEdicion.Visibility = Visibility.Hidden;
                        btnAddBank.Visibility = Visibility.Visible;
                        btnAddBox.Visibility = Visibility.Visible;
                        lblHeader.Content = "Cajas y Bancos";
                    };
                    PanelEdicion.Content = ventana;
                }
            }
        }

        private void btnAddbank_Click(object sender, RoutedEventArgs e)
        {
            DgBoxAndBanks.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            lblHeader.Content = "Nuevo Banco";
            btnAddBank.Visibility = Visibility.Hidden;
            btnAddBox.Visibility = Visibility.Hidden;
            var ventana = new EditBankControlView(0);
            ventana.BancoActualizado += () =>
            {
                DgBoxAndBanks.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAddBank.Visibility = Visibility.Visible;
                btnAddBox.Visibility = Visibility.Visible;
                lblHeader.Content = "Cajas y Bancos";
            };
            PanelEdicion.Content = ventana;
        }

        private void btnAddBox_Click(object sender, RoutedEventArgs e)
        {
            DgBoxAndBanks.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            lblHeader.Content = "Nueva Caja";
            var ventana = new EditBoxControlView(0);
            btnAddBank.Visibility = Visibility.Hidden;
            btnAddBox.Visibility = Visibility.Hidden;
            ventana.CajaActualizada += () =>
            {
                DgBoxAndBanks.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAddBank.Visibility = Visibility.Visible;
                btnAddBox.Visibility = Visibility.Visible;
                lblHeader.Content = "Cajas y Bancos";
            };
            PanelEdicion.Content = ventana;
        }

        private void btnActritations_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Bank bank)
            {
                int bankId = bank.Id;

                // Abrís la ventana de acreditaciones pasando el ID del banco
                //var acreditacionesWindow = new AcreditacionesWindow(bankId);
                //acreditacionesWindow.ShowDialog();
            }
        }

        private void btnDebitations_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Bank bank)
            {
                int bankId = bank.Id;

                // Abrís la ventana de acreditaciones pasando el ID del banco
                //var acreditacionesWindow = new AcreditacionesWindow(bankId);
                //acreditacionesWindow.ShowDialog();
            }
        }
    }
}
