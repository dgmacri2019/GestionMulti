using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters;
using Microsoft.AspNetCore.SignalR;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Client
{
    public class ClientListViewModel : BaseViewModel
    {
        private readonly ClientsApiService _clientsApiService;
        private readonly ClientsHubService _hubService;

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
                    _ = LoadClientsAsync(); // 🔹 ejecuta búsqueda al escribir
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

        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<ClientViewModel> Clients { get; } = new();

        // 🔹 Command para buscar
        public ICommand SearchCommand { get; }
        public ICommand ToggleEnabledCommand { get; }


        public string ToggleEnabledText => IsEnabledFilter? "Clientes Inhabilitados" : "Clientes Habilitados";

        public ClientListViewModel()
        {
            _clientsApiService = new ClientsApiService();

            //var hubUrl = string.Format("{0}hubs/clients", App.Configuration["ApiSettings:ClientsHubUrl"]);
            var hubUrl = string.Format("{0}hubs/clients", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new ClientsHubService(hubUrl);
            _hubService.ClienteCambiado += OnClienteCambiado;            
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadClientsAsync());

            _ = _hubService.StartAsync();
            _ = LoadClientsAsync(); // carga inicial
        }

        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadClientsAsync();
        }

        // 🔹 Carga clientes aplicando filtros
        public async Task LoadClientsAsync()
        {
            if (!ClientCache.Instance.HasData)
            {
                var clients = await _clientsApiService.GetAllAsync();
                ClientCache.Instance.SetClients(clients);
            }

            var filtered = ClientCache.Instance.SearchClients(NameFilter, IsEnabledFilter, IsDeletedFilter);

            App.Current.Dispatcher.Invoke(() =>
            {
                Clients.Clear();
                foreach (var c in filtered)
                    Clients.Add(c);
            });
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnClienteCambiado(ClienteChangeNotification notification)
        {
            var clients = await _clientsApiService.GetAllAsync();

            await App.Current.Dispatcher.InvokeAsync(() =>
            {
                ClientCache.Instance.ClearCache();
                ClientCache.Instance.SetClients(clients);

                _ = LoadClientsAsync();
            });
        }
    }
}
