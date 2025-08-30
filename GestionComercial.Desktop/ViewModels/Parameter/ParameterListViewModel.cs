using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters.Configuration;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.GeneralParameterChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Parameter
{
    internal class ParameterListViewModel : BaseViewModel
    {
        private readonly ParametersApiService _parametersApiService;
        private readonly GeneralParametersHubService _hubService;

        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<GeneralParameter> GeneralParameters { get; } = [];

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


        public ParameterListViewModel()
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
            if (!ParameterCache.Instance.HasDataGeneralParameters)
            {
                List<GeneralParameter> parameters = await _parametersApiService.GetAllGeneralParametersAsync();
                ParameterCache.Instance.SetGeneralParameters(parameters);
            }
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnParametroGeneralCambiado(ParametroGeneralChangeNotification notification)
        {
            List<GeneralParameter> clients = await _parametersApiService.GetAllGeneralParametersAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ParameterCache.Instance.ClearCache();
                ParameterCache.Instance.SetGeneralParameters(clients);

                _ = LoadParametersAsync();
            });
        }

    }
}
