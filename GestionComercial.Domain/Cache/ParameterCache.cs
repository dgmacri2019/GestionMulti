using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;

namespace GestionComercial.Domain.Cache
{
    public class ParameterCache : ICache
    {
        private static ParameterCache? _instance;
        public static ParameterCache Instance => _instance ??= new ParameterCache();

        private GeneralParameter? _generalParameter;

        private PcParameter? _pcSalePointParameter;

        private EmailParameter? _emailParameter;

        private List<PcPrinterParametersListViewModel> _printerParameters;

        private PcPrinterParametersListViewModel? _printerParameter;

        public static bool Reading { get; set; } = false;
        public static bool ReadingOk { get; set; } = false;

        private ParameterCache()
        {
            CacheManager.Register(this);
        }



        public GeneralParameter? GetGeneralParameter()
        {
            return _generalParameter;
        }
        public void SetGeneralParameter(GeneralParameter? generalParameter)
        {
            try
            {
                _generalParameter = generalParameter;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public PcParameter? GetPcParameter()
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



        public EmailParameter? GetEmailParameter()
        {
            return _emailParameter;
        }

        public void SetEmailParameter(EmailParameter? emailParameter)
        {
            try
            {
                _emailParameter = emailParameter;
            }
            catch (Exception)
            {

                throw;
            }
        }





        public void ClearCache()
        {
            if (_printerParameters != null)
                _printerParameters.Clear();
            _generalParameter = null;
            _pcSalePointParameter = null;
            _printerParameter = null;
            _emailParameter = null;
        }


        public bool HasDataGeneralParameter => _generalParameter != null && !Reading;
        public bool HasDataPCParameters => _pcSalePointParameter != null && !Reading;
        public bool HasDataPcPrinterParameter => _printerParameter != null && !Reading;
        public bool HasDataEmailParameter => _emailParameter != null && !Reading;
        public bool HasDataPcPrinterParameters => _printerParameters != null && _printerParameters.Any() && !Reading;
    }
}
