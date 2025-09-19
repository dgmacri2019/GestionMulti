using Afip.PublicServices.Interfaces;
using WSFEHomologacion;

namespace Afip.PublicServices.Services
{
    public class WSFEHomologacionService : IWSFEHomologacionService
    {
        #region Attributes

        private readonly FECAERequest feCAEReq;
        private readonly ServiceSoapClient _serviceSoapClient;
        
        #endregion

        #region Contructor
        public WSFEHomologacionService()
        {
            feCAEReq = new FECAERequest();
            _serviceSoapClient = new ServiceSoapClient();
        }

        #endregion

        #region Public Methods


        #endregion


        #region Private Methods



        #endregion

    }
}
