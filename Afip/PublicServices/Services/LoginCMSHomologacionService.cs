using Afip.PublicServices.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSFEHomologacion;

namespace Afip.PublicServices.Services
{
    public class LoginCMSHomologacionService : ILoginCMSHomologacionService
    {
        public LoginCMSHomologacionService()
        {
                
        }

        public async Task<FEAuthRequest?> LogInWSFEAsync()
        {
            throw new NotImplementedException();
        }
    }
}
