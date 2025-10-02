using System;
using System.ServiceProcess;

namespace GestionComercial.ReportServiceHost
{
    internal static class Program
    {
        static void Main(string[] args)
        {
            if (Environment.UserInteractive) // Si estoy en debug o consola
            {
                var service = new ReportWindowsService();
                service.StartDebug(args); // Modo consola
                Console.WriteLine("Servicio escuchando... Presione ENTER para salir");
                Console.ReadLine();
                service.Stop();
            }
            else
            {
                // Modo real: como servicio de Windows
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new ReportWindowsService()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
