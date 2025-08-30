using GestionComercial.Domain.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.DTOs.Banks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.BoxAndBankChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Bank
{
    internal class BoxAndBankListViewModel : BaseViewModel
    {
        private readonly BanksApiService _bankApiService;
        private readonly BoxAndBanksHubService _hubService;

        public ObservableCollection<BankAndBoxViewModel> BoxAndBanks { get; set; } = [];

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
                    _ = LoadBoxandBanksAsync(); // 🔹 ejecuta búsqueda al escribir
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
        public BoxAndBankListViewModel()
        {
            _bankApiService = new BanksApiService();
            var hubUrl = string.Format("{0}hubs/boxandbank", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new BoxAndBanksHubService(hubUrl);
            _hubService.CajaYBancoCambiado += OnCajaYBancoCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadBoxandBanksAsync());

            _ = _hubService.StartAsync();
            _ = LoadBoxandBanksAsync();
        }


        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadBoxandBanksAsync();
        }

        private async Task LoadBoxandBanksAsync()
        {
            try
            {
                if (!BoxAndBankCache.Instance.HasData)
                {
                    var boxAndBanks = await _bankApiService.SearchAsync();
                    BoxAndBankCache.Instance.SetBoxAndBanks(boxAndBanks);
                }

                var filtered = BoxAndBankCache.Instance.SearchBoxAndBanks(NameFilter, IsEnabledFilter, IsDeletedFilter);
                App.Current.Dispatcher.Invoke(() =>
                {
                    BoxAndBanks.Clear();
                    foreach (var p in filtered)
                        BoxAndBanks.Add(p);
                });
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnCajaYBancoCambiado(CajaYBancoChangeNotification notification)
        {
            var boxAndBank = await _bankApiService.SearchAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                BoxAndBankCache.Instance.ClearCache();
                BoxAndBankCache.Instance.SetBoxAndBanks(boxAndBank);

                _ = LoadBoxandBanksAsync();
            });
        }

    }
}
