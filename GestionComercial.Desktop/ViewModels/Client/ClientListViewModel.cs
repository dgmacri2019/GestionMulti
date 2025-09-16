using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Client
{
    public class ClientListViewModel : BaseViewModel
    {
        private readonly ClientsApiService _clientsApiService;
        private readonly ClientsHubService _hubService;

        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<ClientViewModel> Clients { get; } = [];

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

        // 🔹 Command para buscar
        public ICommand SearchCommand { get; }
        public ICommand ToggleEnabledCommand { get; }


        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitados" : "Ver Habilitados";

        public ClientListViewModel()
        {
            _clientsApiService = new ClientsApiService();
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
                ClientCache.Reading = true;
                ClientResponse clientResponse = await _clientsApiService.GetAllAsync();
                if (clientResponse.Success)
                {
                    ClientCache.Instance.Set(clientResponse.ClientViewModels);
                    //MasterCahe.Instance.SetData(clientResponse.PriceLists, clientResponse.States,
                    //    clientResponse.SaleConditions, clientResponse.IvaConditions, clientResponse.DocumentTypes);
                }
                else
                    MessageBox.Show($"Error al cargar clientes, el error fue:\n{clientResponse.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

                ClientCache.Reading = false;
            }

            var filtered = ClientCache.Instance.Search(NameFilter, IsEnabledFilter, IsDeletedFilter);

            App.Current.Dispatcher.Invoke(() =>
            {
                Clients.Clear();
                foreach (var c in filtered.OrderBy(c => c.BusinessName)
                    .ThenBy(c => c.FantasyName ?? "")
                    .ThenBy(c => c.OptionalCode ?? ""))
                    Clients.Add(c);
            });
        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnClienteCambiado(ClienteChangeNotification notification)
        {
            switch (notification.Action)
            {
                case ChangeType.Created:
                    {
                        if (ClientCache.Instance.FindById(notification.ClientId[0]) == null)
                            await Task.Run(async () => await AgregarCacheAsync(notification.ClientId[0]));
                        break;
                    }
                case ChangeType.Updated:
                    {
                        await Task.Run(async () => await ActualizarCacheAsync(notification.ClientId));
                        break;
                    }
                case ChangeType.Deleted:
                    {
                        await App.Current.Dispatcher.InvokeAsync(async () =>
                        {
                            ClientViewModel? viewModel = ClientCache.Instance.FindById(notification.ClientId[0]);
                            if (viewModel != null)
                            {
                                ClientCache.Instance.Remove(viewModel);
                                //Clients.Remove(viewModel);
                                await LoadClientsAsync();
                            }
                        });
                        break;
                    }
                default:
                    break;
            }
        }




        private async Task AgregarCacheAsync(int clientId)
        {
            ClientResponse clientResponse = await _clientsApiService.GetByIdAsync(clientId);
            if (clientResponse.Success)
                await App.Current.Dispatcher.InvokeAsync(async () =>
                {
                    if (ClientCache.Instance.FindById(clientId) == null)
                        ClientCache.Instance.Set(clientResponse.ClientViewModel);

                    await LoadClientsAsync();
                });
        }

        private async Task ActualizarCacheAsync(List<int> clientsId)
        {
            int cont = 0;
            foreach (var clientId in clientsId)
            {
                ClientResponse clientResponse = await _clientsApiService.GetByIdAsync(clientId);
                if (clientResponse.Success)
                    await App.Current.Dispatcher.InvokeAsync(async () =>
                    {
                        ClientViewModel? viewModel = ClientCache.Instance.FindById(clientId);
                        if (viewModel != null)
                        {
                            ClientCache.Instance.Update(clientResponse.ClientViewModel);
                        }
                    });
                // Reporta progreso
                GlobalProgressHelper.Report(cont + 1, clientsId.Count, $"Procesando clientes {cont} de {clientsId.Count}");
                cont++;
            }
            await GlobalProgressHelper.CompleteAsync();
            await LoadClientsAsync();
        }
    }

}

