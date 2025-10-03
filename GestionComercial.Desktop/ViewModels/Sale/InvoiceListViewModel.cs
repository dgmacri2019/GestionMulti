using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.InvoiceChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Sale
{
    public class InvoiceListViewModel : BaseViewModel
    {
        private readonly InvoicesApiService _invoicesApiService;
        private readonly InvoicesHubService _hubService;

        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<Invoice> Invoices { get; } = [];
        public ObservableCollection<Invoice> InvoicesToday { get; } = [];

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
                    _ = LoadInvoicesAsync(); // 🔹 ejecuta búsqueda al escribir
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


        public InvoiceListViewModel(string superToken = "")
        {
            _invoicesApiService = new InvoicesApiService(superToken);

            //var hubUrl = string.Format("{0}hubs/clients", LoginUserCache.Configuration["ApiSettings:ClientsHubUrl"]);
            var hubUrl = string.Format("{0}hubs/invoices", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new InvoicesHubService(hubUrl);
            _hubService.FacturaCambiado += OnFacturaCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadInvoicesAsync());

            _ = Task.Run(async () =>
            {
                try
                {
                    await _hubService.StartAsync();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error iniciando InvoicesHubService: " + ex);
                    // podés mostrar un mensaje UI o log a archivo
                }
            });
            _ = Task.Run(async () =>
            {
                try
                {
                    await LoadInvoicesAsync();// carga inicial

                    Debug.WriteLine("Iniciando Lista Facturas (LoadInvoicesAsync)");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error iniciando Lista Facturas (LoadInvoicesAsync): " + ex);
                    // podés mostrar un mensaje UI o log a archivo
                }
            });

        }



        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadInvoicesAsync();
        }

        // 🔹 Carga facturas aplicando filtros
        public async Task LoadInvoicesAsync()
        {
            try
            {
                if (!InvoiceCache.Instance.HasData)
                {
                    InvoiceCache.Reading = true;
                    while (!ParameterCache.Instance.HasDataPCParameters)
                        await Task.Delay(10);
                    int salePoint = ParameterCache.Instance.GetPcParameter().SalePoint;
                    InvoiceResponse resultInvoice = await _invoicesApiService.GetAllBySalePointAsync(salePoint);
                    if (!resultInvoice.Success)
                    {
                        MsgBoxAlertHelper.MsgAlertError(resultInvoice.Message);
                        return;
                    }
                    if (resultInvoice.Invoices.Count() == 0)
                        InvoiceCache.ReadingOk = true;

                    List<Invoice> invoices = resultInvoice.Invoices;

                    InvoiceCache.Instance.Set(invoices);
                    InvoiceCache.Reading = false;
                }

                var filtered = InvoiceCache.Instance.GetAll();
                if (filtered != null)
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Invoices.Clear();
                        foreach (var c in filtered)
                            Invoices.Add(c);
                    });
            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);

            }
        }


        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnFacturaCambiado(FacturaChangeNotification notification)
        {
            InvoiceResponse invoiceResponse = await _invoicesApiService.GetAllBySalePointAsync(ParameterCache.Instance.GetPcParameter().SalePoint);
            if (invoiceResponse.Success)
                await App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    InvoiceCache.Instance.ClearCache();
                    InvoiceCache.Instance.Set(invoiceResponse.Invoices);

                    await LoadInvoicesAsync();
                });
            else
                MsgBoxAlertHelper.MsgAlertError(invoiceResponse.Message);
        }


    }
}
