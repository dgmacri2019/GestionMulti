using GestionComercial.Domain.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.DTOs.Stock;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.ArticleChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Stock
{
    public class ArticleListViewModel : BaseViewModel
    {
        private readonly ArticlesApiService _articlesApiService;
        private readonly ArticlesHubService _hubService;

        public ObservableCollection<ArticleViewModel> Articles { get; set; } = [];

        // 🔹 Propiedades de filtros
        private string _nameFilter = string.Empty;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                if (_nameFilter != value)
                {
                    _nameFilter = value;
                    OnPropertyChanged();
                    _ = LoadArticlesAsync(); // 🔹 ejecuta búsqueda al escribir
                }
            }
        }

        private bool _isEnabledFilter = true;
        public bool IsEnabledFilter
        {
            get => _isEnabledFilter;
            set
            {
                if (_isEnabledFilter != value)
                {
                    _isEnabledFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isDeletedFilter = false;
        public bool IsDeletedFilter
        {
            get => _isDeletedFilter;
            set
            {
                if (_isDeletedFilter != value)
                {
                    _isDeletedFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        // 🔹 Command para buscar
        public ICommand SearchCommand { get; }
        public ICommand ToggleEnabledCommand { get; }


        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitados" : "Ver Habilitados";

        public ArticleListViewModel()
        {
            _articlesApiService = new ArticlesApiService();
            var hubUrl = string.Format("{0}hubs/articles", App.Configuration["ApiSettings:BaseUrl"]);

            _hubService = new ArticlesHubService(hubUrl);
            _hubService.ArticuloCambiado += OnArticuloCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadArticlesAsync());

            _ = _hubService.StartAsync();
            _ = LoadArticlesAsync(); // carga inicial
        }


        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadArticlesAsync();
        }




        public async Task LoadArticlesAsync()
        {
            try
            {
                if (!ArticleCache.Instance.HasData)
                {
                    List<ArticleViewModel> articles = await _articlesApiService.GetProductsWithPricesAsync();
                    ArticleCache.Instance.SetArticles(articles);
                }

                var filtered = ArticleCache.Instance.SearchArticles(NameFilter, IsEnabledFilter, IsDeletedFilter);

                App.Current.Dispatcher.Invoke(() =>
                {
                    Articles.Clear();
                    foreach (var p in filtered)
                        Articles.Add(p);
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void OnArticuloCambiado(ArticuloChangeNotification notification)
        {
            List<ArticleViewModel> articles = await _articlesApiService.GetProductsWithPricesAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ArticleCache.Instance.ClearCache();
                ArticleCache.Instance.SetArticles(articles);

                _ = LoadArticlesAsync();
            });
        }


    }
}