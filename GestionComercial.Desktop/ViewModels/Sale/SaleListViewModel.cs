using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;
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

        private DateTime _dateFilter = DateTime.Today.Date;
        public DateTime DateFilter
        {
            get => _dateFilter;
            set
            {
                if (_dateFilter != value)
                {
                    _dateFilter = value;
                    OnPropertyChanged();
                    _ = LoadSalesAsync(value);
                }
            }
        }


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
                    _ = LoadSalesAsync(DateFilter); // 🔹 ejecuta búsqueda al escribir
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
        public ICommand DateChangeCommand { get; }

        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitados" : "Ver Habilitados";


        public SaleListViewModel(string superToken = "")
        {
            _salesApiService = new SalesApiService(superToken);

            //var hubUrl = string.Format("{0}hubs/clients", LoginUserCache.Configuration["ApiSettings:ClientsHubUrl"]);
            var hubUrl = string.Format("{0}hubs/sales", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new SalesHubService(hubUrl);
            _hubService.VentaCambiado += OnVentaCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadSalesAsync(DateFilter));
            DateChangeCommand = new RelayCommand1(async _ => await LoadSalesAsync(DateFilter));

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
                    await LoadSalesAsync(DateTime.Now.Date);// carga inicial

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
            await LoadSalesAsync(DateFilter);
        }

        // 🔹 Carga ventas aplicando filtros
        public async Task LoadSalesAsync(DateTime? dateTime)
        {
            try
            {
                if (!SaleCache.Instance.HasData(dateTime))
                {
                    SaleCache.Reading = true;
                    while (!ParameterCache.Instance.HasDataPCParameters)
                        await Task.Delay(10);
                    int salePoint = ParameterCache.Instance.GetPcParameter().SalePoint;
                    SaleResponse resultSale = await _salesApiService.GetAllBySalePointAsync(salePoint, dateTime);
                    if (!resultSale.Success)
                    {
                        MsgBoxAlertHelper.MsgAlertError(resultSale.Message);
                        return;
                    }
                    if (resultSale.SaleViewModels.Count() == 0)
                        SaleCache.ReadingOk = true;

                    List<SaleViewModel> sales = resultSale.SaleViewModels;

                    SaleCache.Instance.Set(sales);
                    SaleCache.Instance.SetLastSaleNumber(resultSale.LastSaleNumber);
                    SaleCache.Reading = false;
                }

                var filtered = SaleCache.Instance.GetAllSales(dateTime);
                if (filtered != null)
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Sales.Clear();
                        foreach (var c in filtered)
                            Sales.Add(c);
                    });
            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);

            }
        }


        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnVentaCambiado(VentaChangeNotification notification)
        {
            switch (notification.action)
            {
                case ChangeType.Created:
                    {
                        if (SaleCache.Instance.FindById(notification.SaleId) == null)
                            await Task.Run(async () => await AgregarCacheAsync(notification.SaleId));
                        break;
                    }
                case ChangeType.Updated:
                    {
                        await Task.Run(async () => await ActualizarCacheAsync(notification.SaleId));

                        break;
                    }
                default:
                    break;
            }
        }


        private async Task AgregarCacheAsync(int saleId)
        {
            SaleResponse saleResponse = await _salesApiService.GetByIdAsync(saleId);
            if (saleResponse.Success)
                await App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    if (SaleCache.Instance.FindById(saleId) == null)
                        SaleCache.Instance.Set(saleResponse.SaleViewModel);

                    await LoadSalesAsync(saleResponse.SaleViewModel.SaleDate);
                });
        }

        private async Task ActualizarCacheAsync(int saleId)
        {
            GlobalProgressHelper.ReportIndeterminate("Procesando venta");
            SaleResponse saleResponse = await _salesApiService.GetByIdAsync(saleId);
            if (saleResponse.Success)
            {
                await App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    SaleViewModel? viewModel = SaleCache.Instance.FindById(saleId);
                    if (viewModel != null)
                    {
                        SaleCache.Instance.Update(saleResponse.SaleViewModel);
                    }

                });
                await LoadSalesAsync(saleResponse.SaleViewModel.SaleDate);
            }
            await GlobalProgressHelper.CompleteAsync();
        }
    }
}