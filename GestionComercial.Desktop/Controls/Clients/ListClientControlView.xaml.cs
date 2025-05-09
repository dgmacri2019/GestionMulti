using GestionComercial.Desktop.ViewModels.Client;
using GestionComercial.Domain.DTOs.Client;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace GestionComercial.Desktop.Controls.Clients
{
    /// <summary>
    /// Lógica de interacción para ListClientControlView.xaml
    /// </summary>
    public partial class ListClientControlView : UserControl
    {

        private bool Enabled = true;
        private bool Deleted = false;


        public ListClientControlView()
        {
            InitializeComponent();
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            DataContext = new ClientListViewModel(Enabled, Deleted);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgArticles.DataContext = new ClientListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgArticles.SelectedItem is ClientViewModel client)
            {
                DgArticles.Visibility = Visibility.Hidden;
                DgArticles.DataContext = null;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                lblHeader.Content = "Editar Cliente";
                var ventana = new EditClientControlView(client.Id);
                ventana.ClienteActualizado += () =>
                {
                    DgArticles.DataContext = new ClientListViewModel(SearchBox.Text, Enabled, Deleted);
                    DgArticles.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    lblHeader.Content = "Clientes";
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgArticles.Visibility = Visibility.Hidden;
            DgArticles.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Cliente";
            var ventana = new EditClientControlView(0);
            ventana.ClienteActualizado += () =>
            {
                DgArticles.DataContext = new ClientListViewModel(Enabled, Deleted);
                DgArticles.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnDisables.Visibility = Visibility.Visible;
                lblHeader.Content = "Clientes";
            };

            PanelEdicion.Content = ventana;
        }

        private void btnDisables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            Enabled = false;
            DgArticles.DataContext = new ClientListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void btnEnables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            Enabled = true;
            DgArticles.DataContext = new ClientListViewModel(SearchBox.Text, Enabled, Deleted);
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
