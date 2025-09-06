using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
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
        public ObservableCollection<SaleViewModel> SalesToday { get; } = [];

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
            _hubService.SalesChanged += OnSalesChanged;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadSalesAsync());

            _ = Task.Run(async () =>
            {
                try
                {
                    await _hubService.StartAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error iniciando SalesHubService: " + ex);
                    // podés mostrar un mensaje UI o log a archivo
                }
            });
            _ = Task.Run(async () =>
            {
                try
                {
                    await LoadSalesAsync();// carga inicial

                    Debug.WriteLine("Iniciando Lista Ventas (LoadSalesAsync)");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error iniciando Lista Ventas (LoadSalesAsync): " + ex);
                    // podés mostrar un mensaje UI o log a archivo
                }
            });

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
            try
            {
                if (!SaleCache.Instance.HasData)
                {
                    SaleCache.Reading = true;
                    while (!ParameterCache.Instance.HasDataPCParameters)
                        await Task.Delay(10);
                    int salePoint = ParameterCache.Instance.GetPcParameter().SalePoint;
                    SaleResponse resultSale = await _salesApiService.GetAllBySalePointAsync(salePoint);
                    List<SaleViewModel> sales = resultSale.SaleViewModels;

                    SaleCache.Instance.SetSales(sales);
                    SaleCache.Instance.SetLastSaleNumber(resultSale.LastSaleNumber);
                    SaleCache.Reading = false;
                }

                var filtered = SaleCache.Instance.GetAllSales();

                App.Current.Dispatcher.Invoke(() =>
                {
                    Sales.Clear();
                    foreach (var c in filtered)
                        Sales.Add(c);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }


        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnVentaCambiado(VentaChangeNotification notification)
        {
            SaleResponse saleResponse = await _salesApiService.GetAllBySalePointAsync(ParameterCache.Instance.GetPcParameter().SalePoint);
            if(saleResponse.Success)
            await App.Current.Dispatcher.InvokeAsync(async () =>
            {
                SaleCache.Instance.ClearCache();
                SaleCache.Instance.SetSales(saleResponse.SaleViewModels);
                SaleCache.Instance.SetLastSaleNumber(saleResponse.LastSaleNumber);

                await LoadSalesAsync();
            });
        }

        private void OnSalesChanged(string json)
        {
            // acá podés deserializar el objeto si lo necesitas
            // var data = JsonConvert.DeserializeObject<ChangedItem<Sale>>(json);
            // actualizar tu lista o disparar notificación visual
        }
    }
}
