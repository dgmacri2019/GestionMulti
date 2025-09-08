using GestionComercial.Domain.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GestionComercial.Desktop.Controls.Articles
{
    /// <summary>
    /// Lógica de interacción para EditArticleControlView.xaml
    /// </summary>
    public partial class EditArticleControlView : UserControl
    {
        private readonly ArticlesApiService _articlesApiService;
        private readonly int ArticleId;

        private ArticleViewModel articleViewModel { get; set; }

        public event Action ProductoActualizado;


        public EditArticleControlView(int articleId)
        {
            InitializeComponent();
            _articlesApiService = new ArticlesApiService();
            ArticleId = articleId;
            _ = FindArticleAsync();
            if (ArticleId > 0)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
        }

        private async Task FindArticleAsync()
        {
            //ArticleResponse result = await _articlesApiService.GetByIdAsync(ArticleId);
            if (ArticleId == 0)
            {
                articleViewModel = new ArticleViewModel
                {
                    CreateUser = App.UserName
                };
            }
            else
            {
                articleViewModel = ArticleCache.Instance.GetAllArticles().FirstOrDefault(a => a.Id == ArticleId);
                if (articleViewModel != null)
                {
                    //articleViewModel = result.ArticleViewModel;

                    DataContext = articleViewModel;
                }
                else
                    lblError.Text = "No se reconoce el artículo";
            }
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
                articleViewModel.UpdateUser = App.UserName;
                articleViewModel.UpdateDate = DateTime.Now;
                articleViewModel.RealCost = txtRealCost.Text.Substring(0, 1) == "$" ? Convert.ToDecimal(txtRealCost.Text.Substring(1)) : Convert.ToDecimal(txtRealCost.Text);

                Article article = ConverterHelper.ToArticle(articleViewModel, articleViewModel.Id == 0);
                GeneralResponse resultUpdate = await _articlesApiService.UpdateAsync(article);
                if (resultUpdate.Success)
                    ProductoActualizado?.Invoke(); // para notificar a la vista principal
                else
                    lblError.Text = resultUpdate.Message;
                btnUpdate.IsEnabled = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnAdd.IsEnabled = false;
                lblError.Text = string.Empty;
                articleViewModel.RealCost = txtRealCost.Text.Substring(0, 1) == "$" ? Convert.ToDecimal(txtRealCost.Text.Substring(1).Replace(".", ",")) : Convert.ToDecimal(txtRealCost.Text.Replace(".", ","));
                Article article = ConverterHelper.ToArticle(articleViewModel, articleViewModel.Id == 0);
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
        private void TextBox_PreviewMouseLeftButtonDown_SelectAll(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is TextBox tb && !tb.IsKeyboardFocusWithin)
            {
                e.Handled = true; // evita que WPF cambie el foco primero
                tb.Focus();
                tb.SelectAll();
            }
        }


        private void txtStock_LostFocus(object sender, RoutedEventArgs e)
        {
            txtStock.Text = txtStock.Text.Replace(".", ",");
        }
        private void txtStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == "," || textoIngresado == "-")
                result = false;
            else
                result = true;
            e.Handled = result;
        }


        private void txtMinimalStock_LostFocus(object sender, RoutedEventArgs e)
        {
            txtMinimalStock.Text = txtMinimalStock.Text.Replace(".", ",");
        }
        private void txtMinimalStock_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == "," || textoIngresado == "-")
                result = false;
            else
                result = true;
            e.Handled = result;
        }


        private void txtReplacement_LostFocus(object sender, RoutedEventArgs e)
        {
            txtReplacement.Text = txtReplacement.Text.Replace(".", ",");
        }
        private void txtReplacement_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == ",")
                result = false;
            else
                result = true;
            e.Handled = result;
        }


        //private void txtUmbral_LostFocus(object sender, RoutedEventArgs e)
        //{
        //    txtUmbral.Text = txtUmbral.Text.Replace(".", ",");
        //}
        private void txtUmbral_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == ",")
                result = false;
            else
                result = true;
            e.Handled = result;
        }


        private void txtChangePoint_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidatorHelper.IsNumeric(e.Text);
        }


        private void txtSalePoint_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidatorHelper.IsNumeric(e.Text);
        }


        private void txtInternalTax_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !ValidatorHelper.IsNumeric(e.Text);
        }





        private bool EsTextoNumerico(string texto)
        {
            if (texto.Length > 0 && texto.Substring(1).Contains("-"))
                return false;
            if (texto.Substring(0, 1) == "-" && texto.Length == 1)
                return true;
            // Usá CultureInfo si querés permitir coma decimal como en es-AR
            return double.TryParse(texto, NumberStyles.Any, CultureInfo.CurrentCulture, out _);
        }


    }
}
