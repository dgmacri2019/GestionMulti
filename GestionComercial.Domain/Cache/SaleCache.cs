using GestionComercial.Domain.DTOs.Sale;

namespace GestionComercial.Domain.Cache
{
    public class SaleCache : ICache
    {
        private static SaleCache? _instance;
        public static SaleCache Instance => _instance ??= new SaleCache();

        private List<SaleViewModel>? _sales;

        private SaleCache()
        {
            CacheManager.Register(this);
        }


        public List<SaleViewModel>? GetAllSales()
        {
            return _sales;
        }
        public List<SaleViewModel> SearchSales(string clientBusinessName, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _sales != null ? _sales
                              .Where(a => a.IsEnabled == isEnabled
                                       && a.IsDeleted == isDeleted
                                       && ((a.Client.BusinessName?.ToLower().Contains(clientBusinessName.ToLower()) ?? false)
                                        || (a.Client.FantasyName?.ToLower().Contains(clientBusinessName.ToLower()) ?? false)))
                              .ToList()
                              :
                              [];
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void SetSales(List<SaleViewModel> sales)
        {
            try
            {
                _sales = sales;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetSale(SaleViewModel sale)
        {
            try
            {
                _sales.Add(sale);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void UpdateSale(SaleViewModel sale)
        {
            try
            {
                SaleViewModel? saleViewModel = _sales.FirstOrDefault(c => c.Id == sale.Id);
                if (sale != null)
                {
                    _sales.Remove(saleViewModel);
                    _sales.Add(sale);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ClearCache()
        {
            _sales.Clear();
        }

        public int GetNextSaleNumnber(int salePoint)
        {
            return _sales.Where(s => s.SalePoint == salePoint).LastOrDefault() == null ?
                1
                :
                _sales.Where(s => s.SalePoint == salePoint).Max(s => s.SaleNumber) + 1;
        }

        public bool HasData => _sales != null && _sales.Any();
    }
}
