using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using System.Windows;

namespace GestionComercial.Desktop.Views.Searchs
{
    /// <summary>
    /// Lógica de interacción para ArticleSearchWindow.xaml
    /// </summary>
    public partial class ArticleSearchWindow : Window
    {
        private List<ArticleViewModel> _allArticles;

        public ArticleViewModel? SelectedArticle { get; private set; }

        public ArticleSearchWindow(string searchText = "")
        {
            InitializeComponent();

            // Cargar todos los artículos desde tu cache/repositorio
            _allArticles = ArticleCache.Instance.GetAllArticles();

            // Llenar grilla
            dgArticles.ItemsSource = _allArticles;

            // Si vino texto de búsqueda, lo aplico
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                txtSearch.Text = searchText;
                FilterArticles(searchText);
            }
        }

        private void txtSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FilterArticles(txtSearch.Text);
        }

        private void FilterArticles(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                dgArticles.ItemsSource = _allArticles;
            else
                dgArticles.ItemsSource = _allArticles
                    .Where(a => a.Code.ToLower().Contains(text.ToLower()) || a.Description.ToLower().Contains(text.ToLower()))
                    .ToList();
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            SelectedArticle = dgArticles.SelectedItem as ArticleViewModel;
            if (SelectedArticle != null)
                DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void dgArticles_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            BtnOk_Click(sender, e);
        }
    }
}