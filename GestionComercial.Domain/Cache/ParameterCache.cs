using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.Entities.Masters.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GestionComercial.Domain.Cache
{
    public class ParameterCache : ICache
    {
        private static ParameterCache? _instance;
        public static ParameterCache Instance => _instance ??= new ParameterCache();

        private List<GeneralParameter>? _generalParameters;

        private PcParameter? _pcParameter;

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
            return _pcParameter;
        }

        public void SetPCParameter(PcParameter pcParameter)
        {
            try
            {
                _pcParameter = pcParameter;
            }
            catch (Exception)
            {

                throw;
            }
        }






        public void ClearCache()
        {
            _generalParameters?.Clear();
            _pcParameter = null;
        }

        public bool HasDataGeneralParameters => _generalParameters != null && _generalParameters.Any();
        public bool HasDataPCParameters => _pcParameter != null;
    }
}
