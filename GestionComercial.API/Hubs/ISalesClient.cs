using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface ISalesClient
    {
        Task VentasActualizados(VentaChangeNotification notification);
    }

    // Hub tipado
    public class SalesHub : Hub<ISalesClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
