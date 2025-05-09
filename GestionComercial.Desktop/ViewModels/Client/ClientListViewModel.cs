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
                List<ClientViewModel> clients = await _clientsApiService.SearchAsync(name, isEnabled, isDeleted);
                Clients.Clear();
                foreach (var p in clients)
                {
                    Clients.Add(p);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ObservableCollection<ClientViewModel>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            List<ClientViewModel> clients = await _clientsApiService.GetAllAsync(isEnabled, isDeleted);
            Clients.Clear();
            foreach (var p in clients)
            {
                Clients.Add(p);
            }

            return Clients;
        }




    }
}
