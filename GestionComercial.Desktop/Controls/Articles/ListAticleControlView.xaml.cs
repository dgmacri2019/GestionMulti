using GestionComercial.Desktop.ViewModels.Stock;
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


    }
}
