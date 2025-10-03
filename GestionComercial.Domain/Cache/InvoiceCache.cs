using GestionComercial.Domain.Entities.Sales;

namespace GestionComercial.Domain.Cache
{
    public class InvoiceCache : ICache
    {
        private static InvoiceCache? _instance;
        public static InvoiceCache Instance => _instance ??= new InvoiceCache();

        private List<Invoice>? _invoices;

        public static bool Reading { get; set; } = false;
        public static bool ReadingOk { get; set; } = false;

        private InvoiceCache()
        {
            CacheManager.Register(this);
        }


        public List<Invoice>? GetAll()
        {
            return _invoices;
        }


        public void Set(List<Invoice> invoices)
        {
            try
            {
                _invoices = invoices;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Set(Invoice invoice)
        {
            try
            {
                _invoices.Add(invoice);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(Invoice invoice)
        {
            try
            {
                Invoice? invoiceCheck = _invoices.FirstOrDefault(c => c.Id == invoice.Id);
                if (invoiceCheck != null)
                {
                    _invoices.Remove(invoiceCheck);
                    _invoices.Add(invoice);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }



        public bool HasData => _invoices != null && _invoices.Any() && !Reading;

        public void ClearCache()
        {
            try
            {
                if (_invoices != null)
                    _invoices.Clear();
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
