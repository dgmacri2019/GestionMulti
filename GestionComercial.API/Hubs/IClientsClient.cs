using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.API.Hubs
{
    // Interface del "cliente" conectado al hub (lo que el servidor puede invocar en los clientes)
    public interface IClientsClient
    {
        Task ClientesActualizados(ClienteChangeNotification notification);
    }

    // Hub tipado
    public class ClientsHub : Hub<IClientsClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
