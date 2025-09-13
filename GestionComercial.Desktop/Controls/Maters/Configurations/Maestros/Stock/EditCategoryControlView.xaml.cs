using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Views.Auxiliaries;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace GestionComercial.Desktop.Controls.Maters.Configurations.Maestros.Stock
{
    /// <summary>
    /// Lógica de interacción para EditCategoryControlView.xaml
    /// </summary>
    public partial class EditCategoryControlView : UserControl
    {
        private readonly MasterClassApiService _apiService;
        private readonly int CategoryId;

        private CategoryViewModel CategoryViewModel { get; set; }

        public event Action CategoriaActualizada;

        public EditCategoryControlView(int categoryId)
        {
            InitializeComponent();
            _apiService = new MasterClassApiService();
            CategoryId = categoryId;

            if (CategoryId == 0)
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
                CategoryViewModel = new CategoryViewModel { CreateUser = App.UserName, IsEnabled = true };
            }
            else
            {
                CategoryViewModel? category = CategoryCache.Instance.FindById(CategoryId);
                if (category != null)
                {
                    CategoryViewModel = category;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnUpdate.Visibility = Visibility.Visible;
                }
                else
                { }
                //lblError.Text = "No se reconoce el artículo";
            }
            DataContext = CategoryViewModel;
            //txtCode.Focus();
            //txtCode.SelectAll();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // doble clic
            {
                if (sender is Border border && border.DataContext != null)
                {
                    // Suponiendo que Color es un string en formato "#RRGGBB"
                    dynamic item = border.DataContext;

                    var dlg = new ColorPickerWindow(item.Color ?? "#FFFFFF");
                    if (dlg.ShowDialog() == true)
                    {
                        item.Color = dlg.SelectedColorHex; // ejemplo: "#3C3C3C"
                    }

                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CategoriaActualizada?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnUpdate.IsEnabled = false;
                lblError.Text = string.Empty;
                CategoryViewModel.UpdateUser = App.UserName;
                CategoryViewModel.UpdateDate = DateTime.Now;
                GeneralResponse resultUpdate = await _apiService.UpdateCategoryAsync(ConverterHelper.ToCategory(CategoryViewModel, CategoryViewModel.Id == 0));
                if (resultUpdate.Success)
                    CategoriaActualizada?.Invoke(); // para notificar a la vista principal
                else
                    lblError.Text = resultUpdate.Message;
                btnUpdate.IsEnabled = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                btnUpdate.IsEnabled = true;
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAdd.IsEnabled = false;
                lblError.Text = string.Empty;
                GeneralResponse resultUpdate = await _apiService.AddCategoryAsync(ConverterHelper.ToCategory(CategoryViewModel, CategoryViewModel.Id == 0));

                if (resultUpdate.Success)
                    CategoriaActualizada?.Invoke(); // para notificar a la vista principal
                else
                    lblError.Text = resultUpdate.Message;
                btnAdd.IsEnabled = true;
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                btnAdd.IsEnabled = true;
            }
        }


    }
}
