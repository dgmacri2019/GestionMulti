using GestionComercial.ReportServiceHost.Services;
using System.ServiceModel;
using System.ServiceProcess;

namespace GestionComercial.ReportServiceHost
{
    public partial class ReportWindowsService : ServiceBase
    {
        private ServiceHost _serviceHost = null;

        public ReportWindowsService()
        {
            ServiceName = "CrystalReportService";
        }

        protected override void OnStart(string[] args)
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }

            _serviceHost = new ServiceHost(typeof(ReportService));
            _serviceHost.Open();
        }

        protected override void OnStop()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }
    }
}