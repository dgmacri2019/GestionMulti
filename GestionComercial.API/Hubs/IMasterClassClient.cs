using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IMasterClassClient
    {
        Task ClaseMaestraActualizados(ClaseMaestraChangeNotification notification);
    }

    // Hub tipado
    public class MasterClassHub : Hub<IMasterClassClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
