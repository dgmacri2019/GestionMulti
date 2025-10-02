using System.ServiceProcess;

namespace GestionComercial.ReportServiceHost
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ReportWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}