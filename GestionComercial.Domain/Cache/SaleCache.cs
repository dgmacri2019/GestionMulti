using GestionComercial.Domain.DTOs.Sale;

namespace GestionComercial.Domain.Cache
{
    public class SaleCache : ICache
    {
        private static SaleCache? _instance;
        public static SaleCache Instance => _instance ??= new SaleCache();

        private List<SaleViewModel>? _sales = [];

        private int _lastSaleNumber;

        //private List<DateTime?> _chargedSalesDate;


        public static bool Reading { get; set; } = false;
        public static bool ReadingOk { get; set; } = false;

        private SaleCache()
        {
            CacheManager.Register(this);
        }


        public List<SaleViewModel>? GetAllSales(DateTime? dateTime)
        {
            return _sales.Where(s => s.SaleDate == dateTime).ToList();
        }
        public List<SaleViewModel>? GetAllSales()
        {
            return _sales;
        }
        public void Set(List<SaleViewModel> sales)
        {
            try
            {
                _sales.AddRange(sales);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Set(SaleViewModel sale)
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
        public void SetLastSaleNumber(int lastSaleNumber)
        {
            try
            {
                _lastSaleNumber = lastSaleNumber;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int GetLastSaleNumber()
        {
            return _lastSaleNumber;
            //return _sales.Where(s => s.SalePoint == salePoint).LastOrDefault() == null ?
            //    1
            //    :
            //    _sales.Where(s => s.SalePoint == salePoint).Max(s => s.SaleNumber) + 1;
        }
        public void Update(SaleViewModel sale)
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
            try
            {
                if (_sales != null)
                    _sales.Clear();
                if (_lastSaleNumber != 0)
                    _lastSaleNumber = 0;
                //if (_chargedSalesDate != null)
                //    _chargedSalesDate.Clear();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public SaleViewModel? FindBySaleNumber(int salePoint, int saleNumber)
        {
            return _sales.FirstOrDefault(s => s.SalePoint == salePoint && s.SaleNumber == saleNumber);
        }

        public SaleViewModel? FindById(int saleId)
        {
            return _sales.FirstOrDefault(s => s.Id == saleId);
        }


        public bool HasData(DateTime? dateTime)
        {
            bool result = false;
            if (_sales != null)
            {
                if (Reading)
                    result = true;

                if (_sales.Where(s => s.SaleDate == dateTime).Count() > 0)
                    result = true;

            }
            // => _sales.Count != 0

            return result;
        }
    }
}
