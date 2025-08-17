using GestionComercial.Desktop.Cache;
using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Client;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace GestionComercial.Desktop.ViewModels.Client
{
    public class ClientListViewModel : BaseViewModel
    {
        private readonly ClientsApiService _clientsApiService;

        public ObservableCollection<ClientViewModel> Clients { get; set; } = [];

        public ICommand? LoadClientsCommand { get; }

        public ClientListViewModel(bool isEnabled, bool isDeleted)
        {
            _clientsApiService = new ClientsApiService();
            GetAllAsync(isEnabled, isDeleted);
        }


        public ClientListViewModel(string name, bool isEnabled, bool isDeleted)
        {
            _clientsApiService = new ClientsApiService();
            SearchAsync(name, isEnabled, isDeleted);
        }

        private async Task SearchAsync(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                if (!ClientCache.Instance.HasData)
                {
                    List<ClientViewModel> clients = await _clientsApiService.GetAllAsync(isEnabled, isDeleted);
                    ClientCache.Instance.SetClientes(clients);
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
                    List<ClientViewModel> clients = await _clientsApiService.GetAllAsync(isEnabled, isDeleted);
                    ClientCache.Instance.SetClientes(clients);
                }

                Clients.Clear();
                foreach (var p in ClientCache.Instance.GetAllClients())
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




    }
}
