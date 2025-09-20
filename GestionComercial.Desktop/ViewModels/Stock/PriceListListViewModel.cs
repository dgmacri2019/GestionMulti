using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Stock
{
    internal class PriceListListViewModel : BaseViewModel
    {
        private readonly MasterClassApiService _apiService;
        private readonly MasterClassHubService _hubService;

        public ObservableCollection<PriceListViewModel> PriceLists { get; set; } = [];

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
                    _ = LoadPriceListAsync(); // 🔹 ejecuta búsqueda al escribir
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

        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitadas" : "Ver Habilitadas";


        public PriceListListViewModel()
        {
            _apiService = new MasterClassApiService();
            var hubUrl = string.Format("{0}hubs/masterclass", App.Configuration["ApiSettings:BaseUrl"]);
            _hubService = new MasterClassHubService(hubUrl);
            _hubService.ClaseMaestraCambiado += OnListaPrecioCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadPriceListAsync());
            _ = _hubService.StartAsync();
            _ = LoadPriceListAsync(); // carga inicial
        }


        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadPriceListAsync();
        }

        // 🔹 Carga clientes aplicando filtros
        public async Task LoadPriceListAsync()
        {
            if (!PriceListCache.Instance.HasData)
            {
                PriceListCache.Reading = true;
                PriceListResponse result = await _apiService.GetAllPriceListAsync();
                if (result.Success)
                {
                    PriceListCache.Instance.ClearCache();
                    PriceListCache.Instance.Set(result.PriceListViewModels);
                }
                else
                    MsgBoxAlertHelper.MsgAlertError($"Error al cargar listas de precios, el error fue:\n{result.Message}");

                PriceListCache.Reading = false;
            }

            var filtered = PriceListCache.Instance.Search(NameFilter, IsEnabledFilter, IsDeletedFilter);

            App.Current.Dispatcher.Invoke(() =>
            {
                PriceLists.Clear();
                foreach (var c in filtered.OrderBy(c => c.Id))
                    PriceLists.Add(c);
            });
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnListaPrecioCambiado(ClaseMaestraChangeNotification notification)
        {
            if (notification.ChangeClass == ChangeClass.PriceList)
            {
                PriceListResponse result = await _apiService.GetPriceListByIdAsync(notification.Id);
                if (result.Success)
                    switch (notification.Action)
                    {
                        case ChangeType.Created:
                            {
                                if (PriceListCache.Instance.FindById(notification.Id) == null)
                                    await App.Current.Dispatcher.InvokeAsync(async () =>
                                    {
                                        PriceListCache.Instance.Set(result.PriceListViewModel);
                                        await LoadPriceListAsync();
                                    });
                                break;
                            }
                        case ChangeType.Updated:
                            {
                                await App.Current.Dispatcher.InvokeAsync(async () =>
                                {
                                    PriceListViewModel? priceList = PriceListCache.Instance.FindById(notification.Id);
                                    if (priceList != null)
                                    {
                                        PriceListCache.Instance.ClearCache();
                                        //CategoryCache.Instance.Update(category);
                                        await LoadPriceListAsync();
                                    }
                                });
                                break;
                            }
                        case ChangeType.Deleted:
                            {
                                await App.Current.Dispatcher.InvokeAsync(async () =>
                                {
                                    PriceListViewModel? priceList = PriceListCache.Instance.FindById(notification.Id);
                                    if (priceList != null)
                                    {
                                        PriceListCache.Instance.Remove(priceList);
                                        //Clients.Remove(viewModel);
                                        await LoadPriceListAsync();
                                    }
                                });
                                break;
                            }
                        default:
                            break;
                    }
                else
                    MsgBoxAlertHelper.MsgAlertError($"Error al cargar lista de precios, el error fue:\n{result.Message}");
            }
        }
    }
}

