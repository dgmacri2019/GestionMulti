using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Articles
{
    /// <summary>
    /// Lógica de interacción para ProductSaleControlView.xaml
    /// </summary>
    public partial class ListAticleControlView : UserControl
    {

        public ListAticleControlView()
        {           
            InitializeComponent();
            DataContext = new ArticleListViewModel();
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgArticles.SelectedItem is ArticleViewModel article)
            {
                DgArticles.Visibility = Visibility.Hidden;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                lblHeader.Content = "Editar Artículo";
                btnEnables.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                var ventana = new EditArticleControlView(article.Id);
                ventana.ProductoActualizado += () =>
                {
                    DgArticles.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    btnEnables.Visibility = Visibility.Visible;
                    btnAdd.Visibility = Visibility.Visible;
                    lblHeader.Content = "Artículos";
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgArticles.Visibility = Visibility.Hidden;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnEnables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Artículo";
            var ventana = new EditArticleControlView(0);
            ventana.ProductoActualizado += () =>
            {
                DgArticles.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Visible;
                lblHeader.Content = "Artículos";
            };

            PanelEdicion.Content = ventana;
        }

        private void DgArticles_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                bool showPricesWithoutIva = MasterCache.Instance.GetCommerceData() != null
                                    && MasterCache.Instance.GetCommerceData().IvaConditionId == 1;
                bool showCostPrices = false;
                // Buscar columnas por Header o por índice
                DataGridColumn? costoSinIvaCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Costo sin IVA");
                DataGridColumn? bonificacionCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Bonificación");
                DataGridColumn? subtotalSinIvaCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "SubTotal sin IVA");
                DataGridColumn? impuestosCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Impuestos");
                DataGridColumn? costoConIvaCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Costo con IVA");
                DataGridColumn? utilidadCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Utilidad");
                DataGridColumn? ventaSinIvaCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Precio venta sin IVA");
                DataGridColumn? ventaConIvaCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Precio venta con IVA");
                DataGridColumn? ventaCol = DgArticles.Columns.FirstOrDefault(c => (c.Header as TextBlock)?.Text == "Precio venta");

                if (showPricesWithoutIva)
                {
                    if (ventaCol != null) ventaCol.Visibility = Visibility.Collapsed;
                    if (ventaSinIvaCol != null) ventaSinIvaCol.Visibility = Visibility.Visible;
                    if (ventaConIvaCol != null) ventaConIvaCol.Visibility = Visibility.Visible;

                    if (showCostPrices)
                    {
                        if (costoSinIvaCol != null) costoSinIvaCol.Visibility = Visibility.Visible;
                        if (bonificacionCol != null) bonificacionCol.Visibility = Visibility.Visible;
                        if (subtotalSinIvaCol != null) subtotalSinIvaCol.Visibility = Visibility.Visible;
                        if (impuestosCol != null) impuestosCol.Visibility = Visibility.Visible;
                        if (costoConIvaCol != null) costoConIvaCol.Visibility = Visibility.Visible;
                        if (utilidadCol != null) utilidadCol.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (costoSinIvaCol != null) costoSinIvaCol.Visibility = Visibility.Collapsed;
                        if (bonificacionCol != null) bonificacionCol.Visibility = Visibility.Collapsed;
                        if (subtotalSinIvaCol != null) subtotalSinIvaCol.Visibility = Visibility.Collapsed;
                        if (impuestosCol != null) impuestosCol.Visibility = Visibility.Collapsed;
                        if (costoConIvaCol != null) costoConIvaCol.Visibility = Visibility.Collapsed;
                        if (utilidadCol != null) utilidadCol.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    if (ventaSinIvaCol != null) ventaSinIvaCol.Visibility = Visibility.Collapsed;
                    if (ventaConIvaCol != null) ventaConIvaCol.Visibility = Visibility.Collapsed;
                    if (ventaCol != null) ventaCol.Visibility = Visibility.Visible;


                    if (showCostPrices)
                    {
                        //TODO: Resolver Vista para monotributo
                        if (utilidadCol != null) utilidadCol.Visibility = Visibility.Visible;
                        if (impuestosCol != null) impuestosCol.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (costoSinIvaCol != null) costoSinIvaCol.Visibility = Visibility.Collapsed;
                        if (bonificacionCol != null) bonificacionCol.Visibility = Visibility.Collapsed;
                        if (subtotalSinIvaCol != null) subtotalSinIvaCol.Visibility = Visibility.Collapsed;
                        if (impuestosCol != null) impuestosCol.Visibility = Visibility.Collapsed;
                        if (costoConIvaCol != null) costoConIvaCol.Visibility = Visibility.Collapsed;
                        if (utilidadCol != null) utilidadCol.Visibility = Visibility.Collapsed;
                    }

                }

            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);
            }
        }
    }
}
