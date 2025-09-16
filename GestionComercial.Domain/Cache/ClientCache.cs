using GestionComercial.Domain.DTOs.Client;

namespace GestionComercial.Domain.Cache
{
    public class ClientCache : ICache
    {
        private static ClientCache? _instance;
        public static ClientCache Instance => _instance ??= new ClientCache();

        private List<ClientViewModel>? _clients;

        public static bool Reading { get; set; } = false;
        private ClientCache()
        {
            CacheManager.Register(this);
        }

        public List<ClientViewModel> GetAll()
        {
            return _clients.OrderBy(c => c.BusinessName).ToList();
        }
        public List<ClientViewModel> Search(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _clients != null ? _clients
                              .Where(p => p.IsEnabled == isEnabled
                                       && p.IsDeleted == isDeleted
                                       && ((p.BusinessName?.ToLower().Contains(name.ToLower()) ?? false)
                                        || (p.FantasyName?.ToLower().Contains(name.ToLower()) ?? false)
                                        || (p.DocumentNumber?.ToLower().Contains(name.ToLower()) ?? false)))
                              .OrderBy(c => c.BusinessName)
                              .ToList()
                              :
                              new List<ClientViewModel>();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Set(List<ClientViewModel> clients)
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
        public void Set(ClientViewModel client)
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
        public void Update(ClientViewModel client)
        {
            try
            {
                ClientViewModel? clientViewModel = _clients.FirstOrDefault(c => c.Id == client.Id);
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
        public void Remove(ClientViewModel client)
        {
            try
            {
                ClientViewModel? clientViewModel = _clients.FirstOrDefault(c => c.Id == client.Id);
                if (clientViewModel != null)
                    _clients.Remove(clientViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ClientViewModel? FindByOptionalCode(string optionalCode)
        {
            try
            {
                return _clients != null && !string.IsNullOrEmpty(optionalCode) ? _clients
                               .Where(p => p.IsEnabled
                                        && !p.IsDeleted)
                               .FirstOrDefault(a => a.OptionalCode == optionalCode)
                               :
                               null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ClientViewModel? FindById(int id)
        {
            try
            {
                return _clients != null ?
                                _clients.FirstOrDefault(c => c.Id == id)
                               :
                               null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<ClientViewModel>? FindByName(string name)
        {
            try
            {
                return _clients != null && !string.IsNullOrEmpty(name) ? _clients
                             .Where(p => p.IsEnabled
                                      && !p.IsDeleted
                                      && ((p.BusinessName?.ToLower().Contains(name.ToLower()) ?? false)
                                       || (p.FantasyName?.ToLower().Contains(name.ToLower()) ?? false)))
                             .OrderBy(c => c.BusinessName)
                             .ToList()
                             :
                             null;
            }
            catch (Exception)
            {
                throw;
            }
        }





        public void ClearCache()
        {
            _clients?.Clear();
        }

        public bool HasData => _clients != null && _clients.Any() && !Reading;
    }
}
