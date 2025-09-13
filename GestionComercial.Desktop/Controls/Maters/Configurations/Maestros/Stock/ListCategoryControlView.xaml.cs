using GestionComercial.Desktop.ViewModels.Stock;
using GestionComercial.Domain.DTOs.Stock;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock
{
    /// <summary>
    /// Lógica de interacción para ListCategoryControlView.xaml
    /// </summary>
    public partial class ListCategoryControlView : UserControl
    {
        public ListCategoryControlView()
        {
            InitializeComponent();
            DataContext = new CategoryListViewModel();
        }


        private void DataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DgCategories.SelectedItem is CategoryViewModel category)
            {
                DgCategories.Visibility = Visibility.Hidden;
                PanelSearch.Visibility = Visibility.Hidden;
                PanelEdicion.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Hidden;
                lblHeader.Content = "Editar Rubro";
                var ventana = new EditCategoryControlView(category.Id);
                ventana.CategoriaActualizada += () =>
                {
                    DgCategories.Visibility = Visibility.Visible;
                    PanelSearch.Visibility = Visibility.Visible;
                    PanelEdicion.Content = null;
                    PanelEdicion.Visibility = Visibility.Hidden;
                    btnAdd.Visibility = Visibility.Visible;
                    btnEnables.Visibility = Visibility.Visible;
                    lblHeader.Content = "Rubros";
                    if (!string.IsNullOrEmpty(SearchBox.Text))
                        SearchBox.Focus();
                };

                PanelEdicion.Content = ventana;

            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DgCategories.Visibility = Visibility.Hidden;
            DgCategories.DataContext = null;
            PanelSearch.Visibility = Visibility.Hidden;
            PanelEdicion.Visibility = Visibility.Visible;
            btnEnables.Visibility = Visibility.Hidden;
            btnAdd.Visibility = Visibility.Hidden;
            lblHeader.Content = "Nuevo Rubro";
            var ventana = new EditCategoryControlView(0);
            ventana.CategoriaActualizada += () =>
            {
                DgCategories.Visibility = Visibility.Visible;
                PanelSearch.Visibility = Visibility.Visible;
                PanelEdicion.Content = null;
                PanelEdicion.Visibility = Visibility.Hidden;
                btnAdd.Visibility = Visibility.Visible;
                btnEnables.Visibility = Visibility.Visible;
                lblHeader.Content = "Rubros";
            };

            PanelEdicion.Content = ventana;
        }





    }
}
