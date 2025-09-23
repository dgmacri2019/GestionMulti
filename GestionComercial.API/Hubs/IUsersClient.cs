using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.UserChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IUsersClient
    {
        Task UsuariosActualizados(UsuarioChangeNotification notification);
    }

    // Hub tipado
    public class UsersHub : Hub<IUsersClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
