using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using System.Windows.Controls;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock
{
    /// <summary>
    /// Lógica de interacción para EditCategoryControlView.xaml
    /// </summary>
    public partial class EditCategoryControlView : UserControl
    {
        private readonly MasterClassApiService _apiService;
        private readonly int CategoryId;

        private Category Category { get; set; }

        public event Action CategoriaActualizada;

        public EditCategoryControlView(int categoryId)
        {
            InitializeComponent();
            _apiService = new MasterClassApiService();
            CategoryId = categoryId;

            if (CategoryId == 0)
            {
                //btnAdd.Visibility = Visibility.Visible;
                //btnUpdate.Visibility = Visibility.Hidden;
                Category = new Category { CreateUser = App.UserName, IsEnabled = true };
            }
            else
            {
                Category? category = CategoryCache.Instance.FindById(CategoryId);
                if (category != null)
                {
                    Category = category;
                    //btnAdd.Visibility = Visibility.Hidden;
                    //btnUpdate.Visibility = Visibility.Visible;
                }
                else
                { }
                    //lblError.Text = "No se reconoce el artículo";
            }
            DataContext = Category;
            //txtCode.Focus();
            //txtCode.SelectAll();
        }
    }
}
