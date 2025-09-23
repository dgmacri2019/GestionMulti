using GestionComercial.Domain.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.DTOs.Provider;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.ProviderChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Provider
{
    internal class ProviderListViewModel : BaseViewModel
    {
        private readonly ProvidersApiService _providersApiService;
        private readonly ProvidersHubService _hubService;
        public ObservableCollection<ProviderViewModel> Providers { get; set; } = [];

        // 🔹 Command para buscar
        public ICommand SearchCommand { get; }
        public ICommand ToggleEnabledCommand { get; }

        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitados" : "Ver Habilitados";

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
                    _ = LoadProvidersAsync(); // 🔹 ejecuta búsqueda al escribir
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

        public ProviderListViewModel()
        {
            _providersApiService = new ProvidersApiService();
            //var hubUrl = string.Format("{0}hubs/clients", LoginUserCache.Configuration["ApiSettings:ClientsHubUrl"]);
            var hubUrl = string.Format("{0}hubs/providers", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new ProvidersHubService(hubUrl);
            _hubService.ProveedorCambiado += OnProveedorCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadProvidersAsync());

            _ = _hubService.StartAsync();
            _ = LoadProvidersAsync(); // carga inicial
        }

        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadProvidersAsync();
        }

        // 🔹 Carga clientes aplicando filtros
        public async Task LoadProvidersAsync()
        {
            if (!ProviderCache.Instance.HasData)
            {
                var providers = await _providersApiService.GetAllAsync();
                ProviderCache.Instance.SetProviders(providers);
            }

            var filtered = ProviderCache.Instance.SearchProviders(NameFilter, IsEnabledFilter, IsDeletedFilter);

            App.Current.Dispatcher.Invoke(() =>
            {
                Providers.Clear();
                foreach (var c in filtered)
                    Providers.Add(c);
            });
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnProveedorCambiado(ProveedorChangeNotification notification)
        {
            var providers = await _providersApiService.GetAllAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ProviderCache.Instance.ClearCache();
                ProviderCache.Instance.SetProviders(providers);

                _ = LoadProvidersAsync();
            });
        }
    }
}
