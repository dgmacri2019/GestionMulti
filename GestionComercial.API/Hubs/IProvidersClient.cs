using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.ProviderChangeNotification;

namespace GestionComercial.API.Hubs
{
    // Interface del "cliente" conectado al hub (lo que el servidor puede invocar en los clientes)
    public interface IProvidersClient
    {
        Task ProveedoresActualizados(ProveedorChangeNotification notification);
    }

    // Hub tipado
    public class ProvidersHub : Hub<IProvidersClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}

