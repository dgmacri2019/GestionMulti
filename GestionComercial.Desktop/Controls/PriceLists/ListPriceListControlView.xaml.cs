using GestionComercial.Desktop.ViewModels.PriceList;
using GestionComercial.Domain.DTOs.PriceLists;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.PriceLists
{
    /// <summary>
    /// Lógica de interacción para ListPriceListControlView.xaml
    /// </summary>
    public partial class ListPriceListControlView : UserControl
    {
        private bool Enabled = true;
        private bool Deleted = false;

        public ListPriceListControlView()
        {
            InitializeComponent();
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            DataContext = new PriceListListViewModel(Enabled, Deleted);
        }



        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgPriceLists.DataContext = new PriceListListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgPriceLists.SelectedItem is PriceListViewModel client)
            {
                DgPriceLists.Visibility = Visibility.Hidden;
                DgPriceLists.DataContext = null;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                btnDisables.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                lblHeader.Content = "Editar Lista de Precios";
                var ventana = new EditPriceListControlView(client.Id);
                ventana.ListaPrecioActualizada += () =>
                {
                    DgPriceLists.DataContext = new PriceListListViewModel(SearchBox.Text, Enabled, Deleted);
                    DgPriceLists.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Visible;
                    btnDisables.Visibility = Visibility.Visible;
                    lblHeader.Content = "Lista de Precios";
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgPriceLists.Visibility = Visibility.Hidden;
            DgPriceLists.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nueva Lista de Precios";
            var ventana = new EditPriceListControlView(0);
            ventana.ListaPrecioActualizada += () =>
            {
                DgPriceLists.DataContext = new PriceListListViewModel(Enabled, Deleted);
                DgPriceLists.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnDisables.Visibility = Visibility.Visible;
                lblHeader.Content = "Lista de Precios";
            };

            PanelEdicion.Content = ventana;
        }

        private void btnDisables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            Enabled = false;
            DgPriceLists.DataContext = new PriceListListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void btnEnables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            Enabled = true;
            DgPriceLists.DataContext = new PriceListListViewModel(SearchBox.Text, Enabled, Deleted);
        }


    }
}
