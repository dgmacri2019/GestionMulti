using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Domain.DTOs.PriceLists;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock
{
    /// <summary>
    /// Lógica de interacción para ListPriceListControlView.xaml
    /// </summary>
    public partial class ListPriceListControlView : UserControl
    {
        public ListPriceListControlView()
        {
            InitializeComponent();
            DataContext = new PriceListListViewModel();
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgPriceLists.SelectedItem is PriceListViewModel priceList)
            {
                DgPriceLists.Visibility = Visibility.Hidden;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                lblHeader.Content = "Editar Lista de Precios";
                var ventana = new EditPriceListControlView(priceList.Id);
                ventana.ListaPrecioActualizada += () =>
                {
                    DgPriceLists.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Visible;
                    btnEnables.Visibility = Visibility.Visible;
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
            //DgPriceLists.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnEnables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nueva Lista de Precios";
            var ventana = new EditPriceListControlView(0);
            ventana.ListaPrecioActualizada += () =>
            {
                DgPriceLists.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Visible;
                lblHeader.Content = "Lista de Precios";
            };

            PanelEdicion.Content = ventana;
        }       
    }
}
