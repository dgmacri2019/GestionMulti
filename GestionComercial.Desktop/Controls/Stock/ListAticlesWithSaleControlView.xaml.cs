using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Domain.DTOs.Stock;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Stock
{
    /// <summary>
    /// Lógica de interacción para ProductSaleControlView.xaml
    /// </summary>
    public partial class ListAticlesWithSaleControlView : UserControl
    {
        private bool Enabled = true;
        private bool Deleted = false;

        public ListAticlesWithSaleControlView()
        {
            InitializeComponent();
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            DataContext = new ArticleListViewModel(Enabled, Deleted);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            DgArticles.DataContext = new ArticleListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgArticles.SelectedItem is ArticleWithPricesDto article)
            {
                DgArticles.Visibility = Visibility.Hidden;
                DgArticles.DataContext = null;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                lblHeader.Content = "Editar Artículo";
                var ventana = new EditArticleControlView(article.Id);
                ventana.ProductoActualizado += () =>
                {
                    DgArticles.DataContext = new ArticleListViewModel(Enabled, Deleted);
                    DgArticles.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    lblHeader.Content = "Artículos";
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
            lblHeader.Content = "Nuevo Artículo";
            var ventana = new EditArticleControlView(0);
            ventana.ProductoActualizado += () =>
            {
                DgArticles.DataContext = new ArticleListViewModel(Enabled, Deleted);
                DgArticles.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                lblHeader.Content = "Artículos";
            };

            PanelEdicion.Content = ventana;
        }

        private void btnDisables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Visible;
            btnDisables.Visibility = Visibility.Hidden;
            Enabled = false;
            DgArticles.DataContext = new ArticleListViewModel(SearchBox.Text, Enabled, Deleted);
        }

        private void btnEnables_Click(object sender, RoutedEventArgs e)
        {
            btnEnables.Visibility = Visibility.Hidden;
            btnDisables.Visibility = Visibility.Visible;
            Enabled = true;
            DgArticles.DataContext = new ArticleListViewModel(SearchBox.Text, Enabled, Deleted);
        }
    }
}
