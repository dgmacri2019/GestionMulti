using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Stock
{
    public class CategoryListViewModel : BaseViewModel
    {
        private readonly MasterClassApiService _apiService;
        private readonly MasterClassHubService _hubService;



        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<CategoryViewModel> Categories { get; } = [];

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
                    _ = LoadCategoriesAsync(); // 🔹 ejecuta búsqueda al escribir
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



        public CategoryListViewModel()
        {
            _apiService = new MasterClassApiService();
            var hubUrl = string.Format("{0}hubs/masterclass", App.Configuration["ApiSettings:BaseUrl"]);
            _hubService = new MasterClassHubService(hubUrl);
            _hubService.ClaseMaestraCambiado += OnCategoriaCambiado;

            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadCategoriesAsync());

            _ = _hubService.StartAsync();
            _ = LoadCategoriesAsync(); // carga inicial
        }


        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadCategoriesAsync();
        }

        // 🔹 Carga clientes aplicando filtros
        public async Task LoadCategoriesAsync()
        {
            if (!CategoryCache.Instance.HasData)
            {
                CategoryCache.Reading = true;
                CategoryResponse result = await _apiService.GetAllCategoriesAsync();
                if (result.Success)
                {
                    CategoryCache.Instance.ClearCache();
                    CategoryCache.Instance.Set(result.Categories);
                }
                else
                    MsgBoxAlertHelper.MsgAlertError($"Error al cargar categorias, el error fue:\n{result.Message}");

                CategoryCache.Reading = false;
            }

            var filtered = CategoryCache.Instance.Search(NameFilter, IsEnabledFilter, IsDeletedFilter);

            App.Current.Dispatcher.Invoke(() =>
            {
                Categories.Clear();
                foreach (var c in filtered.OrderBy(c => c.Description))
                    Categories.Add(c);
            });
        }


        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnCategoriaCambiado(ClaseMaestraChangeNotification notification)
        {
            if (notification.ChangeClass == ChangeClass.Category)
            {
                CategoryResponse categoryResponse = await _apiService.GetCategoryByIdAsync(notification.Id);
                if (categoryResponse.Success)
                    switch (notification.Action)
                    {
                        case ChangeType.Created:
                            {
                                await App.Current.Dispatcher.InvokeAsync(async () =>
                                {
                                    CategoryCache.Instance.Set(categoryResponse.Category);
                                    await LoadCategoriesAsync();
                                });
                                break;
                            }
                        case ChangeType.Updated:
                            {
                                await App.Current.Dispatcher.InvokeAsync(async () =>
                                {
                                    CategoryViewModel? category = CategoryCache.Instance.FindById(notification.Id);
                                    if (category != null)
                                    {
                                        CategoryCache.Instance.ClearCache();
                                        //CategoryCache.Instance.Update(category);
                                        await LoadCategoriesAsync();
                                    }
                                });
                                break;
                            }
                        case ChangeType.Deleted:
                            {
                                await App.Current.Dispatcher.InvokeAsync(async () =>
                                {
                                    CategoryViewModel? category = CategoryCache.Instance.FindById(notification.Id);
                                    if (category != null)
                                    {
                                        CategoryCache.Instance.Remove(category);
                                        //Clients.Remove(viewModel);
                                        await LoadCategoriesAsync();
                                    }
                                });
                                break;
                            }
                        default:
                            break;
                    }
            }
        }

    }
}
