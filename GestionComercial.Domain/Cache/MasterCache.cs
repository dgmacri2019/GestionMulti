using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using System.Xml.Linq;

namespace GestionComercial.Domain.Cache
{
    public class MasterCache : ICache
    {

        private static MasterCache _instance;

        public static MasterCache Instance => _instance ??= new MasterCache();

        private List<State> _states;
        private List<SaleCondition> _saleConditions;
        private List<IvaCondition> _ivaConditions;
        private List<DocumentType> _documentTypes;
        private List<Measure> _measures;
        private List<Tax> _taxes;

        public static bool Reading { get; set; } = false;


        public MasterCache()
        {
            CacheManager.Register(this);
        }


        public List<Tax> GetTaxes()
        {
            return _taxes != null ? _taxes.ToList() : [];
        }
        public List<Measure> GetMeasures()
        {
            return _measures != null ? _measures.ToList() : [];
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

        public void SetData(List<State> states, List<SaleCondition> saleConditions,
            List<IvaCondition> ivaConditions, List<DocumentType> documentTypes, List<Measure> measures,
            List<Tax> taxes)
        {
            _states = states;
            _saleConditions = saleConditions;
            _ivaConditions = ivaConditions;
            _documentTypes = documentTypes;
            _taxes = taxes;
            _measures = measures;
        }

        public bool HasData => ((_states != null && _states.Any()) || (_saleConditions != null && _saleConditions.Any())
            || (_ivaConditions != null && _ivaConditions.Any()) || (_documentTypes != null && _documentTypes.Any()) || (_taxes != null && _taxes.Any())
            || (_measures != null && _measures.Any())) && !Reading;

        public void ClearCache()
        {
            try
            {
                _states.Clear();
                _saleConditions.Clear();
                _ivaConditions.Clear();
                _documentTypes.Clear();
                _taxes.Clear();
                _measures.Clear();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }
}
