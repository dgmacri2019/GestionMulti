using GestionComercial.Domain.DTOs.Client;

namespace GestionComercial.Desktop.Cache
{
    public class ClientCache
    {
        private static ClientCache _instance;
        public static ClientCache Instance => _instance ??= new ClientCache();

        private List<ClientViewModel> _clients;

        public List<ClientViewModel> GetAllClients()
        {
            return _clients;
        }

        public List<ClientViewModel> SearchClients(string name, bool isEnabled, bool isDeleted)
        {
            return _clients
                    .Where(p => p.IsEnabled == isEnabled
                             && p.IsDeleted == isDeleted
                             && ((p.BusinessName?.ToLower().Contains(name.ToLower()) ?? false)
                              || (p.FantasyName?.ToLower().Contains(name.ToLower()) ?? false)
                              || (p.DocumentNumber?.ToLower().Contains(name.ToLower()) ?? false)))
                    .ToList();
        }




        public void SetClientes(List<ClientViewModel> clients)
        {
            _clients = clients;
        }

        public bool HasData => _clients != null && _clients.Any();
    }
}
