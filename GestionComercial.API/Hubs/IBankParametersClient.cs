using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.BankParameterChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IBankParametersClient
    {
        Task ParametrosBancariosActualizados(ParametroBancarioChangeNotification notification);
    }

    // Hub tipado
    public class BankParametersHub : Hub<IBankParametersClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
