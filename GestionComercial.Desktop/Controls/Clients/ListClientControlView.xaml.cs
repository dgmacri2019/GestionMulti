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

        public ListClientControlView()
        {
            InitializeComponent();
            //btnEnables.Visibility = Visibility.Hidden;
            //btnDisables.Visibility = Visibility.Visible;
            DataContext = new ClientListViewModel();
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgArticles.SelectedItem is ClientViewModel client)
            {
                DgArticles.Visibility = Visibility.Hidden;
                //DgArticles.DataContext = null;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                lblHeader.Content = "Editar Cliente";
                btnEnables.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                var ventana = new EditClientControlView(client.Id);
                ventana.ClienteActualizado += () =>
                {
                    //DgArticles.DataContext = new ClientListViewModel();
                    //DataContext = new ClientListViewModel();
                    DgArticles.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    lblHeader.Content = "Clientes";
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
            DgArticles.Visibility = Visibility.Hidden;
            //DgArticles.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnEnables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Cliente";
            var ventana = new EditClientControlView(0);
            ventana.ClienteActualizado += () =>
            {
                //DgArticles.DataContext = new ClientListViewModel();
                DgArticles.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Visible;
                lblHeader.Content = "Clientes";
            };

            PanelEdicion.Content = ventana;
            //PanelEdicion.MaxWidth = this.Width;
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
