using GestionComercial.Domain.DTOs.Provider;

namespace GestionComercial.Domain.Cache
{
    public class ProviderCache : ICache
    {
        private static ProviderCache _instance;
        public static ProviderCache Instance => _instance ??= new ProviderCache();

        private List<ProviderViewModel> _providers;
        private ProviderCache()
        {
            CacheManager.Register(this);
        }
        public List<ProviderViewModel> GetAllProviders()
        {
            return _providers;
        }

        public List<ProviderViewModel> SearchProviders(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _providers != null ? _providers
                              .Where(p => p.IsEnabled == isEnabled
                                       && p.IsDeleted == isDeleted
                                       && ((p.BusinessName?.ToLower().Contains(name.ToLower()) ?? false)
                                        || (p.FantasyName?.ToLower().Contains(name.ToLower()) ?? false)
                                        || (p.DocumentNumber?.ToLower().Contains(name.ToLower()) ?? false)))
                              .ToList()
                              :
                              [];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetProviders(List<ProviderViewModel> providers)
        {
            try
            {
                _providers = providers;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetProvider(ProviderViewModel provider)
        {
            try
            {
                _providers.Add(provider);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateProvider(ProviderViewModel provider)
        {
            try
            {
                ProviderViewModel providerViewModel = _providers.FirstOrDefault(c => c.Id == provider.Id);
                if (provider != null)
                {
                    _providers.Remove(providerViewModel);
                    _providers.Add(provider);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void RemoveProvider(ProviderViewModel provider)
        {
            try
            {
                ProviderViewModel providerViewModel = _providers.FirstOrDefault(c => c.Id == provider.Id);
                if (providerViewModel != null)
                    _providers.Remove(providerViewModel);
            }
            catch (Exception)
            {

                throw;
            }

        }


        public void ClearCache()
        {
            _providers.Clear();
        }

        public bool HasData => _providers != null && _providers.Any();
    }
}
