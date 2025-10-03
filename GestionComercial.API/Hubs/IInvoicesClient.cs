using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.InvoiceChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IInvoicesClient
    {
        Task FacturasActualizados(FacturaChangeNotification notification);
    }

    // Hub tipado
    public class InvoiceHub : Hub<IInvoicesClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
