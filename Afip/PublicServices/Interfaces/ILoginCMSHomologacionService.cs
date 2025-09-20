using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSFEHomologacion;

namespace Afip.PublicServices.Interfaces
{
    public interface ILoginCMSHomologacionService
    {
        Task<FEAuthRequest?> LogInWSFEAsync();
    }
}
