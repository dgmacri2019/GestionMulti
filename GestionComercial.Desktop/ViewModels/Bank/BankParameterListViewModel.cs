using GestionComercial.Domain.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.DTOs.Banks;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.BankParameterChangeNotification;
using static GestionComercial.Domain.Notifications.BoxAndBankChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Bank
{
    internal class BankParameterListViewModel : BaseViewModel
    {
        private readonly BanksApiService _bankApiService;
        private readonly BankParametersHubService _hubService;

        public ObservableCollection<BankParameterViewModel> BankParameters { get; set; } = [];

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
                    _ = LoadBankParametersAsync(); // 🔹 ejecuta búsqueda al escribir
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
        public BankParameterListViewModel()
        {
            _bankApiService = new BanksApiService();
            var hubUrl = string.Format("{0}hubs/bankparameter", App.Configuration["ApiSettings:BaseUrl"]);

            _hubService = new BankParametersHubService(hubUrl);
            _hubService.ParametrosBancariosCambiado += OnParametroCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadBankParametersAsync());

            _ = _hubService.StartAsync();
            _ = LoadBankParametersAsync();
        }

        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadBankParametersAsync();
        }

        private async Task LoadBankParametersAsync()
        {
            try
            {
                if (!BankParameterCache.Instance.HasData)
                {
                    List<BankParameterViewModel> bankParameterViewModels = await _bankApiService.SearchBankParameterAsync();
                    BankParameterCache.Instance.SetBankParameters(bankParameterViewModels);
                }

                var filtered = BankParameterCache.Instance.SearchBankParameters(NameFilter, IsEnabledFilter, IsDeletedFilter);
                App.Current.Dispatcher.Invoke(() =>
                {
                    BankParameters.Clear();
                    foreach (var p in filtered)
                        BankParameters.Add(p);
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnParametroCambiado(ParametroBancarioChangeNotification notification)
        {
            var boxAndBank = await _bankApiService.SearchBankParameterAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                BankParameterCache.Instance.ClearCache();
                BankParameterCache.Instance.SetBankParameters(boxAndBank);

                _ = LoadBankParametersAsync();
            });
        }
    }
}
