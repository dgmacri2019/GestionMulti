using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Client;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Client
{
    public class ClientListViewModel : BaseViewModel
    {
        private readonly ClientsApiService _clientsApiService;

        public ObservableCollection<ClientViewModel> Clients { get; set; } = [];

        public ICommand? LoadClientsCommand { get; }

        private readonly ClientsHubService _hubService;

        public ClientListViewModel(bool isEnabled, bool isDeleted)
        {
            _clientsApiService = new ClientsApiService();
            var hubUrl = App.Configuration["ApiSettings:ClientsHubUrl"];
            _hubService = new ClientsHubService(hubUrl);

            _hubService.ClienteCambiado += OnClienteCambiado;

            // Iniciamos la conexión
            _ = _hubService.StartAsync();
            _ = GetAllAsync(isEnabled, isDeleted);
        }


        public ClientListViewModel(string name, bool isEnabled, bool isDeleted)
        {
            _clientsApiService = new ClientsApiService();
            var hubUrl = App.Configuration["ApiSettings:ClientsHubUrl"];
            _hubService = new ClientsHubService(hubUrl);

            _hubService.ClienteCambiado += OnClienteCambiado;

            // Iniciamos la conexión
            _ = _hubService.StartAsync();
            _ = SearchAsync(name, isEnabled, isDeleted);
        }

        private async Task SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                if (!ClientCache.Instance.HasData)
                {
                    List<ClientViewModel> clients = await _clientsApiService.GetAllAsync();
                    ClientCache.Instance.SetClients(clients);
                }
                Clients.Clear();
                foreach (var p in ClientCache.Instance.SearchClients(name, isEnabled, isDeleted))
                {
                    Clients.Add(p);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<ObservableCollection<ClientViewModel>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            try
            {
                if (!ClientCache.Instance.HasData)
                {
                    List<ClientViewModel> clients = await _clientsApiService.GetAllAsync();
                    ClientCache.Instance.SetClients(clients);
                }

                Clients.Clear();
                foreach (var p in ClientCache.Instance.GetAllClients().Where(c => c.IsEnabled == isEnabled && c.IsDeleted == isDeleted).ToList())
                {
                    Clients.Add(p);
                }
                return Clients;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void OnClienteCambiado(ClienteChangeNotification noti)
        {
            //ClientResponse clientResponse;
            List<ClientViewModel> clients;
            // Actualizamos la lista según el tipo de cambio
            switch (noti)
            {
                case ClientCreado c:
                    //clientResponse = await _clientsApiService.GetByIdAsync(c.ClientId);
                    //if (clientResponse.Success)
                    //{
                    //    ClientCache.Instance.SetClient(clientResponse.ClientViewModel);
                    //}
                    //else
                    //{
                    clients = await _clientsApiService.GetAllAsync();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ClientCache.Instance.ClearCache();
                        //Clients.Clear();
                        ClientCache.Instance.SetClients(clients);
                        //}

                    });
                    break;
                case ClientActualizado c:
                    clients = await _clientsApiService.GetAllAsync();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ClientCache.Instance.ClearCache();
                        //Clients.Clear();
                        ClientCache.Instance.SetClients(clients);
                        //}

                    });
                    //var existente = Clients.FirstOrDefault(x => x.Id == c.ClientId);
                    //if (existente != null)
                    //{
                    //    clientResponse = await _clientsApiService.GetByIdAsync(c.ClientId);
                    //    if (clientResponse.Success)
                    //    {
                    //        ClientCache.Instance.UpdateClient(clientResponse.ClientViewModel);
                    //    }
                    //    else
                    //    {
                    //        ClientCache.Instance.ClearCache();
                    //        Clients.Clear();
                    //    }
                    //}
                    break;
                case ClientEliminado c:
                    clients = await _clientsApiService.GetAllAsync();
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        ClientCache.Instance.ClearCache();
                        //Clients.Clear();
                        ClientCache.Instance.SetClients(clients);
                        //}

                    });
                    break;
            }
        }



    }
}
