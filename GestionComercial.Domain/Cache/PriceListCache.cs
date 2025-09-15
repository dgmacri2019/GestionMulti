using GestionComercial.Domain.DTOs.PriceLists;

namespace GestionComercial.Domain.Cache
{
    public class PriceListCache : ICache
    {
        private static PriceListCache? _instance;
        public static PriceListCache Instance => _instance ??= new PriceListCache();

        private List<PriceListViewModel> _priceLists;
        public static bool Reading { get; set; } = false;

        public bool HasData => _priceLists != null && _priceLists.Any() && !Reading;

        public PriceListCache()
        {
            CacheManager.Register(this);
        }


        public List<PriceListViewModel> GetAll()
        {
            return _priceLists.OrderBy(c => c.Description).ToList();
        }
        public List<PriceListViewModel> Search(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _priceLists != null ? _priceLists
                              .Where(p => p.IsEnabled == isEnabled
                                       && p.IsDeleted == isDeleted)
                              .OrderBy(c => c.Description)
                              .ToList()
                              :
                              [];
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Set(List<PriceListViewModel> categories)
        {
            try
            {
                _priceLists = categories;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Set(PriceListViewModel client)
        {
            try
            {
                _priceLists.Add(client);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(PriceListViewModel categoryNew)
        {
            try
            {
                PriceListViewModel? category = _priceLists.FirstOrDefault(c => c.Id == categoryNew.Id);
                if (category != null)
                {
                    _priceLists.Remove(category);
                    _priceLists.Add(categoryNew);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Remove(PriceListViewModel client)
        {
            try
            {
                PriceListViewModel? Category = _priceLists.FirstOrDefault(c => c.Id == client.Id);
                if (Category != null)
                    _priceLists.Remove(Category);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public PriceListViewModel? FindById(int id)
        {
            try
            {
                return _priceLists != null ?
                                _priceLists.FirstOrDefault(c => c.Id == id)
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
            _priceLists?.Clear();
        }
    }
}
