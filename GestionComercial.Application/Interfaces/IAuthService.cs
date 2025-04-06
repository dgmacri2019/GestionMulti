using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> Authenticate(string username, string password);
    }
}
