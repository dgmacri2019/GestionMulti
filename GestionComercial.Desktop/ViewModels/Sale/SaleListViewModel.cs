using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Sale;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Sale
{
    public class SaleListViewModel : BaseViewModel
    {
        private readonly SalesApiService _salesApiService;
        private readonly SalesHubService _hubService;

        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<SaleViewModel> Sales { get; } = [];

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
                    _ = LoadSalesAsync(); // 🔹 ejecuta búsqueda al escribir
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


        public SaleListViewModel()
        {
            _salesApiService = new SalesApiService();

            //var hubUrl = string.Format("{0}hubs/clients", App.Configuration["ApiSettings:ClientsHubUrl"]);
            var hubUrl = string.Format("{0}hubs/sales", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new SalesHubService(hubUrl);
            _hubService.VentaCambiado += OnVentaCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadSalesAsync());

            _ = _hubService.StartAsync();
            _ = LoadSalesAsync(); // carga inicial
        }



        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadSalesAsync();
        }

        // 🔹 Carga clientes aplicando filtros
        public async Task LoadSalesAsync()
        {
            if (!SaleCache.Instance.HasData)
            {
                List<SaleViewModel> sales = await _salesApiService.GetAllAsync();
                SaleCache.Instance.SetSales(sales);
            }

            var filtered = SaleCache.Instance.SearchSales(NameFilter, IsEnabledFilter, IsDeletedFilter);

            App.Current.Dispatcher.Invoke(() =>
            {
                Sales.Clear();
                foreach (var c in filtered)
                    Sales.Add(c);
            });
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnVentaCambiado(VentaChangeNotification notification)
        {
            List<SaleViewModel> sales = await _salesApiService.GetAllAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                SaleCache.Instance.ClearCache();
                SaleCache.Instance.SetSales(sales);

                _ = LoadSalesAsync();
            });
        }
    }
}
