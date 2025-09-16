using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ArticleChangeNotification;
using static System.Net.Mime.MediaTypeNames;

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
                    ArticleCache.Reading = true;

                    ArticleResponse articleResponse = await _articlesApiService.GetAllAsync();
                    if (articleResponse.Success)
                        ArticleCache.Instance.Set(articleResponse.ArticleViewModels);
                    else
                        MessageBox.Show($"Error al cargar articulos, el error fue:\n{articleResponse.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);


                    ArticleCache.Reading = false;
                }

                List<ArticleViewModel> filtered = ArticleCache.Instance.Search(NameFilter, IsEnabledFilter, IsDeletedFilter);

                App.Current.Dispatcher.Invoke(() =>
                {
                    Articles.Clear();
                    foreach (var p in filtered
                                        .OrderBy(a => a.Category)
                                        .ThenBy(a => a.Code)
                                        .ThenBy(a => a.Description))

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
            switch (notification.action)
            {
                case ChangeType.Created:
                    {
                        if (ArticleCache.Instance.FindById(notification.ClientId[0]) == null)
                            await Task.Run(async () => await AgregarCacheAsync(notification.ClientId[0]));
                        break;
                    }
                case ChangeType.Updated:
                    {
                        await Task.Run(async () => await ActualizarCacheAsync(notification.ClientId));

                        break;
                    }
                case ChangeType.Deleted:
                    {
                        await App.Current.Dispatcher.InvokeAsync(async () =>
                        {
                            ArticleViewModel? viewModel = ArticleCache.Instance.FindById(notification.ClientId[0]);
                            if (viewModel != null)
                            {
                                ArticleCache.Instance.Remove(viewModel);
                                await LoadArticlesAsync();
                            }
                        });
                        break;
                    }
                default:
                    break;
            }
        }

        private async Task AgregarCacheAsync(int articleId)
        {
            ArticleResponse articleResponse = await _articlesApiService.GetByIdAsync(articleId);
            if (articleResponse.Success)
                await App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    if (ArticleCache.Instance.FindById(articleId) == null)
                        ArticleCache.Instance.Set(articleResponse.ArticleViewModel);

                    await LoadArticlesAsync();
                });
        }

        private async Task ActualizarCacheAsync(List<int> articlesId)
        {
            if (articlesId.Count < 20)
            {
                int cont = 0;
                foreach (var clientId in articlesId)
                {
                    ArticleResponse articleResponse = await _articlesApiService.GetByIdAsync(clientId);
                    if (articleResponse.Success)
                        await App.Current.Dispatcher.InvokeAsync(async () =>
                        {
                            ArticleViewModel? viewModel = ArticleCache.Instance.FindById(clientId);
                            if (viewModel != null)
                            {
                                ArticleCache.Instance.Update(articleResponse.ArticleViewModel);
                            }
                        });
                    // Reporta progreso
                    GlobalProgressHelper.Report(cont + 1, articlesId.Count, $"Procesando artículos {cont} de {articlesId.Count}");
                    cont++;
                }
                await GlobalProgressHelper.CompleteAsync();
            }
            else
            {
                GlobalProgressHelper.ReportIndeterminate("Procesando artículos");
                ArticleResponse articleResponse = await _articlesApiService.GetAllAsync();
                if (articleResponse.Success)
                    await App.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        ArticleCache.Instance.ClearCache();
                        ArticleCache.Instance.Set(articleResponse.ArticleViewModels);

                    });
                await GlobalProgressHelper.CompleteAsync();
            }
            await LoadArticlesAsync();
        }
    }
}