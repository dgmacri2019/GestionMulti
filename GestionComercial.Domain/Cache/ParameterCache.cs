using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;

namespace GestionComercial.Domain.Cache
{
    public class ParameterCache : ICache
    {
        private static ParameterCache? _instance;
        public static ParameterCache Instance => _instance ??= new ParameterCache();

        private List<GeneralParameter>? _generalParameters;

        private PcParameter? _pcSalePointParameter;

        private List<PcPrinterParametersListViewModel> _printerParameters;
        private PcPrinterParametersListViewModel? _printerParameter;

        public static bool Reading { get; set; } = false;

        private ParameterCache()
        {
            CacheManager.Register(this);
        }



        public List<GeneralParameter> GetAllGeneralParameters()
        {
            return _generalParameters;
        }
        public void SetGeneralParameters(List<GeneralParameter> generalParameters)
        {
            try
            {
                _generalParameters = generalParameters;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public PcParameter GetPcParameter()
        {
            return _pcSalePointParameter;
        }

        public void SetPCParameter(PcParameter pcParameter)
        {
            try
            {
                _pcSalePointParameter = pcParameter;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<PcPrinterParametersListViewModel> GetAllPrinterParameters()
        {
            return _printerParameters;
        }

        public PcPrinterParametersListViewModel? GetPrinterParameter()
        {
            return _printerParameter;
        }



        public void SetPrinterParameters(List<PcPrinterParametersListViewModel> printerParameters)
        {
            try
            {
                _printerParameters = printerParameters;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void SetPrinterParameter(PcPrinterParametersListViewModel? printerParameter)
        {
            try
            {
                _printerParameter = printerParameter;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void ClearCache()
        {
            if (_generalParameters != null)
                _generalParameters?.Clear();
            if (_printerParameters != null)
                _printerParameters.Clear();
            _pcSalePointParameter = null;
            _printerParameter = null;
        }

        public bool HasDataGeneralParameters => _generalParameters != null && _generalParameters.Any() && !Reading;
        public bool HasDataPCParameters => _pcSalePointParameter != null && !Reading;
        public bool HasDataPcPrinterParameters => _printerParameters != null && !Reading;
    }
}
