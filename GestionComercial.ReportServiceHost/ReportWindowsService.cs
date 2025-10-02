using GestionComercial.ReportServiceHost.Services;
using System;
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
            StartServiceHost();
        }

        public void StartDebug(string[] args)
        {
            StartServiceHost();

            // Solo mostramos en consola si está en modo debug
            Console.WriteLine("Servicio escuchando en:");
            foreach (var endpoint in _serviceHost.Description.Endpoints)
            {
                Console.WriteLine($"  {endpoint.Address.Uri}");
            }
        }

        private void StartServiceHost()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
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
