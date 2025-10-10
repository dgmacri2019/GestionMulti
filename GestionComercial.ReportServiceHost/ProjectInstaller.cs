using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace ServiceWSDL
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            serviceInstaller1.BeforeInstall += Autorun_BeforeInstall;
            serviceInstaller1.AfterInstall += Autorun_AfterServiceInstall;
        }

        void Autorun_AfterServiceInstall(object sender, InstallEventArgs e)
        {
            ServiceInstaller serviceInstaller = (ServiceInstaller)sender;
            using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
            {
                //if (sc.Status == ServiceControllerStatus.Running)
                //    sc.Stop();
                if (sc.Status == ServiceControllerStatus.Stopped)
                    sc.Start();
            }
        }

        void Autorun_BeforeInstall(object sender, InstallEventArgs e)
        {
            ServiceInstaller serviceInstaller = (ServiceInstaller)sender;
            using (ServiceController sc = new ServiceController(serviceInstaller.ServiceName))
            {
                if (sc.Status == ServiceControllerStatus.Running)
                    sc.Stop();
                //if (sc.Status == ServiceControllerStatus.Stopped)
                //    sc.Start();
            }
        }




    }
}
