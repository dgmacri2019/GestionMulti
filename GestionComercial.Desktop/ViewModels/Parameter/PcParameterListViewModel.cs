using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.GeneralParameterChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Parameter
{
    internal class PcParameterListViewModel : BaseViewModel
    {
        private readonly ParametersApiService _parametersApiService;
        private readonly GeneralParametersHubService _hubService;


        public ObservableCollection<PcSalePointsListViewModel> SalePointsListViewModels { get; } = [];
        public ObservableCollection<PcPrinterParametersListViewModel> PcPrintersListViewModels { get; } = [];
        public PcPrinterParametersListViewModel PcPrintersListViewModel { get; set; }


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
                    _ = LoadParametersAsync(); // 🔹 ejecuta búsqueda al escribir
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


        public PcParameterListViewModel()
        {
            var hubUrl = string.Format("{0}hubs/generalparameter", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new GeneralParametersHubService(hubUrl);
            _hubService.ParametroGeneralCambiado += OnParametroGeneralCambiado;
            //ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            //SearchCommand = new RelayCommand1(async _ => await LoadClientsAsync());

            _parametersApiService = new ParametersApiService();
            _ = _hubService.StartAsync();
            _ = LoadParametersAsync(); // carga inicial
        }


        // 🔹 Carga clientes aplicando filtros
        public async Task LoadParametersAsync()
        {
            if (!ParameterCache.Instance.HasDataGeneralParameter)
            {
                ParameterCache.Reading = true;
                GeneralParameter? generalParameter = await _parametersApiService.GetGeneralParameterAsync();
                ParameterCache.Instance.SetGeneralParameter(generalParameter);
                ParameterCache.Reading = false;
            }
            if (!ParameterCache.Instance.HasDataPCParameters)
            {
                ParameterCache.Reading = true;
                PcParameter pcParameter = await _parametersApiService.GetPcParameterAsync(Environment.MachineName);
                ParameterCache.Instance.SetPCParameter(pcParameter);

                ParameterCache.Reading = false;
            }
            if (!ParameterCache.Instance.HasDataPcPrinterParameters)
            {
                ParameterCache.Reading = true;
                List<PcPrinterParametersListViewModel> printerParameters = await _parametersApiService.GetAllPcPrinterParametersAsync();
                ParameterCache.Instance.SetPrinterParameters(printerParameters);
                ParameterCache.Reading = false;
            }
            if(!ParameterCache.Instance.HasDataPcPrinterParameter)
            {
                ParameterCache.Reading = true;
                PcPrinterParametersListViewModel? printerParameter = await _parametersApiService.GetPrinterParameterFromPcAsync(Environment.MachineName);
                ParameterCache.Instance.SetPrinterParameter(printerParameter);
                ParameterCache.Reading = false;
            }
            if(!ParameterCache.Instance.HasDataEmailParameter)
            {
                ParameterCache.Reading = true;
                EmailParameter? emailParameter = await _parametersApiService.GetEmailParameterAsync();
                ParameterCache.Instance.SetEmailParameter(emailParameter);
                ParameterCache.Reading = false;
            }

            

            List<PcSalePointsListViewModel> salePoints = await _parametersApiService.GetAllPcParametersAsync();
            List<PcPrinterParametersListViewModel> pcPrinterParameters = await _parametersApiService.GetAllPcPrinterParametersAsync();

            App.Current.Dispatcher.Invoke(() =>
            {
                SalePointsListViewModels.Clear();
                PcPrintersListViewModels.Clear();
                foreach (var c in salePoints)
                    SalePointsListViewModels.Add(c);
                foreach (var x in pcPrinterParameters)
                    PcPrintersListViewModels.Add(x);
            });
            ParameterCache.ReadingOk = true;
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnParametroGeneralCambiado(ParametroGeneralChangeNotification notification)
        {
            GeneralParameter? generalParameter = await _parametersApiService.GetGeneralParameterAsync();
            PcParameter pcSalePointParameter = await _parametersApiService.GetPcParameterAsync(Environment.MachineName);
            List<PcPrinterParametersListViewModel> printerParameters = await _parametersApiService.GetAllPcPrinterParametersAsync();
            PcPrinterParametersListViewModel? printerParameter = await _parametersApiService.GetPrinterParameterFromPcAsync(Environment.MachineName);
            EmailParameter? emailParameter = await _parametersApiService.GetEmailParameterAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ParameterCache.Instance.ClearCache();
                ParameterCache.Instance.SetGeneralParameter(generalParameter);
                ParameterCache.Instance.SetPCParameter(pcSalePointParameter);
                ParameterCache.Instance.SetPrinterParameters(printerParameters);
                ParameterCache.Instance.SetPrinterParameter(printerParameter);
                ParameterCache.Instance.SetEmailParameter(emailParameter);
                _ = LoadParametersAsync();
            });
        }

    }
}
