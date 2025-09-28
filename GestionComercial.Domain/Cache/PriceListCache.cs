using GestionComercial.Domain.DTOs.PriceLists;

namespace GestionComercial.Domain.Cache
{
    public class PriceListCache : ICache
    {
        private static PriceListCache? _instance;
        public static PriceListCache Instance => _instance ??= new PriceListCache();

        private List<PriceListViewModel?>? _priceLists;
        public static bool Reading { get; set; } = false;

        public bool HasData => _priceLists != null && _priceLists.Any() && !Reading;

        public PriceListCache()
        {
            CacheManager.Register(this);
        }


        public List<PriceListViewModel?>? GetAll()
        {
            return _priceLists.OrderBy(c => c.Description).ToList();
        }
        public List<PriceListViewModel?>? Search(string name, bool isEnabled, bool isDeleted)
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
        public void Set(List<PriceListViewModel?>? priceLists)
        {
            try
            {
                _priceLists = priceLists;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Set(PriceListViewModel? priceList)
        {
            try
            {
                _priceLists.Add(priceList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(PriceListViewModel? priceListNew)
        {
            try
            {
                PriceListViewModel? priceListOld = _priceLists.FirstOrDefault(c => c.Id == priceListNew.Id);
                if (priceListOld != null)
                {
                    _priceLists.Remove(priceListOld);
                    _priceLists.Add(priceListNew);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Remove(PriceListViewModel? priceList)
        {
            try
            {
                PriceListViewModel? priceListOld = _priceLists.FirstOrDefault(c => c.Id == priceList.Id);
                if (priceListOld != null)
                    _priceLists.Remove(priceListOld);
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
