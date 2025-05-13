using GestionComercial.Desktop.ViewModels.Bank;
using GestionComercial.Domain.DTOs.Bank;
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
        private bool Enabled = true;
        private bool Deleted = false;

        public ListBankControlView()
        {
            InitializeComponent();
            //btnEnables.Visibility = Visibility.Hidden;
            //btnDisables.Visibility = Visibility.Visible;
            DataContext = new BoxAndBankListViewModel(string.Empty, Enabled, Deleted);
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgBoxAndBanks.SelectedItem is BankAndBoxViewModel bankAndBoxViewModel)
            {
                DgBoxAndBanks.Visibility = Visibility.Hidden;
                DgBoxAndBanks.DataContext = null;
                PanelEdicion.Visibility = Visibility.Visible;
                btnAddBank.Visibility = Visibility.Hidden;
                btnAddBox.Visibility = Visibility.Hidden;
                if (bankAndBoxViewModel.IsBank)
                {
                    lblHeader.Content = "Editar Banco";
                    var ventana = new EditBankControlView(bankAndBoxViewModel.Id);
                    ventana.BancoActualizado += () =>
                    {
                        DgBoxAndBanks.DataContext = new BoxAndBankListViewModel(string.Empty, Enabled, Deleted);
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
                        DgBoxAndBanks.DataContext = new BoxAndBankListViewModel(string.Empty, Enabled, Deleted);
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
            DgBoxAndBanks.DataContext = null;
            PanelEdicion.Visibility = Visibility.Visible;
            lblHeader.Content = "Nuevo Banco";
            var ventana = new EditBankControlView(0);
            ventana.BancoActualizado += () =>
            {
                DgBoxAndBanks.DataContext = new BoxAndBankListViewModel(string.Empty, Enabled, Deleted);
                DgBoxAndBanks.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                lblHeader.Content = "Cajas y Bancos";
            };
            PanelEdicion.Content = ventana;
        }

        private void btnAddBox_Click(object sender, RoutedEventArgs e)
        {
            DgBoxAndBanks.Visibility = Visibility.Hidden;
            DgBoxAndBanks.DataContext = null;
            PanelEdicion.Visibility = Visibility.Visible;
            lblHeader.Content = "Nueva Caja";
            var ventana = new EditBoxControlView(0);
            ventana.CajaActualizada += () =>
            {
                DgBoxAndBanks.DataContext = new BoxAndBankListViewModel(string.Empty, Enabled, Deleted);
                DgBoxAndBanks.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
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
