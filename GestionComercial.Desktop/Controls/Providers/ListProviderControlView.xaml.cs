using GestionComercial.Desktop.ViewModels.Provider;
using GestionComercial.Domain.DTOs.Provider;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GestionComercial.Desktop.Controls.Providers
{
    /// <summary>
    /// Lógica de interacción para ListProviderControlView.xaml
    /// </summary>
    public partial class ListProviderControlView : UserControl
    {
        private bool Enabled = true;
        private bool Deleted = false;

        public ListProviderControlView()
        {
            InitializeComponent();
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            DataContext = new ProviderListViewModel(Enabled, Deleted);
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgProviders.DataContext = new ProviderListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgProviders.SelectedItem is ProviderViewModel client)
            {
                DgProviders.Visibility = Visibility.Hidden;
                DgProviders.DataContext = null;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                lblHeader.Content = "Editar Proveedor";
                var ventana = new EditProviderControlView(client.Id);
                ventana.ProveedorActualizado += () =>
                {
                    DgProviders.DataContext = new ProviderListViewModel(SearchBox.Text, Enabled, Deleted);
                    DgProviders.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    lblHeader.Content = "Proveedores";
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgProviders.Visibility = Visibility.Hidden;
            DgProviders.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Proveedor";
            var ventana = new EditProviderControlView(0);
            ventana.ProveedorActualizado += () =>
            {
                DgProviders.DataContext = new ProviderListViewModel(Enabled, Deleted);
                DgProviders.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnDisables.Visibility = Visibility.Visible;
                lblHeader.Content = "Proveedores";
            };

            PanelEdicion.Content = ventana;
        }

        private void btnDisables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            Enabled = false;
            DgProviders.DataContext = new ProviderListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void btnEnables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            Enabled = true;
            DgProviders.DataContext = new ProviderListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void Email_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo($"mailto:{e.Uri}")
            {
                UseShellExecute = true
            });
            e.Handled = true;
        }

        private void WebSite_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo($"{e.Uri}")
            {
                UseShellExecute = true
            });
            e.Handled = true;
        }
    }
}
