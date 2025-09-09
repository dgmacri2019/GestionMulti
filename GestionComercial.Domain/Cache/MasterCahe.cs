using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.Xml.Linq;

namespace GestionComercial.Domain.Cache
{
    public class MasterCahe : ICache
    {

        private static MasterCahe _instance;

        public static MasterCahe Instance => _instance ??= new MasterCahe();

        private List<PriceList> _priceLists;
        private List<State> _states;
        private List<SaleCondition> _saleConditions;
        private List<IvaCondition> _ivaConditions;
        private List<DocumentType> _documentTypes;

        public MasterCahe()
        {
            CacheManager.Register(this);
        }


        public List<PriceList> GetPriceLists()
        {
            return _priceLists != null ? _priceLists.ToList() : [];
        }

        public List<State> GetStates()
        {
            return _states != null ? _states.ToList() : [];
        }

        public List<SaleCondition> GetSaleConditions()
        {
            return _saleConditions != null ? _saleConditions.ToList() : [];
        }

        public List<IvaCondition> GetIvaConditions()
        {
            return _ivaConditions != null ? _ivaConditions.ToList() : [];
        }

        public List<DocumentType> GetDocumentTypes()
        {
            return _documentTypes != null ? _documentTypes.ToList() : [];
        }

        public void SetData(List<PriceList> priceLists, List<State> states, List<SaleCondition> saleConditions,
            List<IvaCondition> ivaConditions, List<DocumentType> documentTypes)
        {
            _priceLists = priceLists;
            _states = states;
            _saleConditions = saleConditions;
            _ivaConditions = ivaConditions;
            _documentTypes = documentTypes;
        }

        public bool HasData => (_priceLists != null && _priceLists.Any()) || (_states != null && _states.Any()) || (_saleConditions != null && _saleConditions.Any())
            || (_ivaConditions != null && _ivaConditions.Any()) || (_documentTypes != null && _documentTypes.Any());

        public void ClearCache()
        {
            _priceLists.Clear();
            _states.Clear();
            _saleConditions.Clear();
            _ivaConditions.Clear();
            _documentTypes.Clear();
        }

    }
}
