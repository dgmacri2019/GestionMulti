using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Articles
{
    /// <summary>
    /// Lógica de interacción para EditArticleControlView.xaml
    /// </summary>
    public partial class EditArticleControlView : UserControl
    {
        private readonly ArticlesApiService _articlesApiService;
        private readonly int ArticleId;

        private ArticleViewModel ArticleViewModel { get; set; }

        public event Action ProductoActualizado;


        public EditArticleControlView(int articleId)
        {
            InitializeComponent();
            _articlesApiService = new ArticlesApiService();
            ArticleId = articleId;

            if (ArticleId == 0)
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
                ArticleViewModel = new ArticleViewModel { CreateUser = App.UserName, IsEnabled = true };
            }
            else
            {
                ArticleViewModel? viewModel = ArticleCache.Instance.GetAll().FirstOrDefault(a => a.Id == ArticleId);
                if (viewModel != null)
                {
                    ArticleViewModel = viewModel;
                    btnAdd.Visibility = Visibility.Hidden;
                    btnUpdate.Visibility = Visibility.Visible;
                }
                else
                    lblError.Text = "No se reconoce el artículo";
            }
            if (ArticleViewModel != null)
            {
                ArticleViewModel.Taxes = MasterCache.Instance.GetTaxes();
                ArticleViewModel.Measures = MasterCache.Instance.GetMeasures();
                var categories = CategoryCache.Instance.GetAll();
                ArticleViewModel.Categories = [.. categories];
            }
            DataContext = ArticleViewModel;
            txtCode.Focus();
            txtCode.SelectAll();
        }
        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblError.MaxWidth = this.ActualWidth;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ProductoActualizado?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnUpdate.IsEnabled = false;
                lblError.Text = string.Empty;
                ArticleViewModel.UpdateUser = App.UserName;
                ArticleViewModel.UpdateDate = DateTime.Now;
                //articleViewModel.RealCost = txtRealCost.Text.Substring(0, 1) == "$" ? Convert.ToDecimal(txtRealCost.Text.Substring(1)) : Convert.ToDecimal(txtRealCost.Text);
                Article article = ConverterHelper.ToArticle(ArticleViewModel, ArticleViewModel.Id == 0);
                GeneralResponse resultUpdate = await _articlesApiService.UpdateAsync(article);
                if (resultUpdate.Success)
                    ProductoActualizado?.Invoke(); // para notificar a la vista principal
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
                //articleViewModel.RealCost = txtRealCost.Text.Substring(0, 1) == "$" ? Convert.ToDecimal(txtRealCost.Text.Substring(1).Replace(".", ",")) : Convert.ToDecimal(txtRealCost.Text.Replace(".", ","));
                Article article = ConverterHelper.ToArticle(ArticleViewModel, ArticleViewModel.Id == 0);
                GeneralResponse resultUpdate = await _articlesApiService.AddAsync(article);
                if (resultUpdate.Success)
                    ProductoActualizado?.Invoke(); // para notificar a la vista principal
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

        private void TextBox_SelectAll(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox tb)
            {
                tb.SelectAll();
            }
        }

        // Salta al siguiente control al presionar Enter
        private void TextBox_KeyDown_MoveNext(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true; // evita el sonido del Enter
                if (sender is UIElement element)
                {
                    element.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }
            }
        }

        // Selecciona todo al hacer click con el mouse (si aún no tenía foco)
        private void TextBox_PreviewMouseLeftButtonDown_SelectAll(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true; // evita que WPF cambie el foco primero
                tb.Focus();
                tb.SelectAll();
            }
        }

        private void TextBoxIsNumeric_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidatorHelper.IsNumeric(e.Text);
        }


    }
}
