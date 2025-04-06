using GestionComercial.Desktop.ViewModels;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls
{
    /// <summary>
    /// Lógica de interacción para ProductSaleControlView.xaml
    /// </summary>
    public partial class ProductSaleControlView : UserControl
    {
        public ProductSaleControlView()
        {
            InitializeComponent();
            DataContext = new ProductListViewModel();
        }


    }
}
