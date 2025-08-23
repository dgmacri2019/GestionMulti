using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.BoxAndBankChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IBoxAndBanksClient
    {
        Task CajasYBancosActualizados(CajaYBancoChangeNotification notification);
    }

    // Hub tipado
    public class BoxAndBanksHub : Hub<IBoxAndBanksClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
