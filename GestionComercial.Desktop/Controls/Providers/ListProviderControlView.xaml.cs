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
        public ListProviderControlView()
        {
            InitializeComponent();
            DataContext = new ProviderListViewModel();
        }


        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgProviders.DataContext = new ProviderListViewModel();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgProviders.SelectedItem is ProviderViewModel provider)
            {
                DgProviders.Visibility = Visibility.Hidden;
                //DgProviders.DataContext = null;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                lblHeader.Content = "Editar Proveedor";
                btnAdd.Visibility = Visibility.Hidden;
                var ventana = new EditProviderControlView(provider.Id);
                ventana.ProveedorActualizado += () =>
                {
                    //DgProviders.DataContext = new ProviderListViewModel();
                    DgProviders.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    lblHeader.Content = "Proveedores";
                    btnEnables.Visibility = Visibility.Visible;
                    btnAdd.Visibility = Visibility.Visible;
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgProviders.Visibility = Visibility.Hidden;
            //DgProviders.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnEnables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Proveedor";
            var ventana = new EditProviderControlView(0);
            ventana.ProveedorActualizado += () =>
            {
                //DgProviders.DataContext = new ProviderListViewModel();
                DgProviders.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Visible;
                lblHeader.Content = "Proveedores";
            };

            PanelEdicion.Content = ventana;
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
