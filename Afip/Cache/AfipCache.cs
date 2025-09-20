using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
using GestionComercial.Domain.Entities.Masters;

namespace Afip.Cache
{
    internal class AfipCache
    {
        private static AfipCache _instance;

        public static AfipCache Instance => _instance ??= new AfipCache();

        private CommerceData? _commerceData;
        private Billing? _billing;

        public static bool Reading { get; set; } = false;

        public Billing? GetBilling()
        {
            return _billing;
        }

        public CommerceData? GetCommerceData()
        {
            return _commerceData;
        }


        public void SetData(CommerceData? commerceData, Billing billing)
        {
            _commerceData = commerceData;
            _billing = billing;
        }


        public bool HasData => _commerceData != null && _billing != null && !Reading;


    }
}
