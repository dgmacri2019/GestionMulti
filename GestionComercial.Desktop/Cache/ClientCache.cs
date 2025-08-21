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
            try
            {
                return _clients != null ? _clients
                              .Where(p => p.IsEnabled == isEnabled
                                       && p.IsDeleted == isDeleted
                                       && ((p.BusinessName?.ToLower().Contains(name.ToLower()) ?? false)
                                        || (p.FantasyName?.ToLower().Contains(name.ToLower()) ?? false)
                                        || (p.DocumentNumber?.ToLower().Contains(name.ToLower()) ?? false)))
                              .ToList()
                              :
                              new List<ClientViewModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetClients(List<ClientViewModel> clients)
        {
            try
            {
                _clients = clients;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetClient(ClientViewModel client)
        {
            try
            {
                _clients.Add(client);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateClient(ClientViewModel client)
        {
            try
            {
                ClientViewModel clientViewModel = _clients.FirstOrDefault(c => c.Id == client.Id);
                if (clientViewModel != null)
                {
                    _clients.Remove(clientViewModel);
                    _clients.Add(client);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void RemoveClient(ClientViewModel client)
        {
            try
            {
                ClientViewModel clientViewModel = _clients.FirstOrDefault(c => c.Id == client.Id);
                if (clientViewModel != null)
                    _clients.Remove(clientViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void ClearCache()
        {
            _clients.Clear();
        }

        public bool HasData => _clients != null && _clients.Any();
    }
}
