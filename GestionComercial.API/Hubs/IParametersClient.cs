using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.GeneralParameterChangeNotification;

namespace GestionComercial.API.Hubs
{
    public interface IParametersClient
    {
        Task ParametrosGeneralesActualizados(ParametroGeneralChangeNotification notification);
    }

    // Hub tipado
    public class GeneralParametersHub : Hub<IParametersClient>
    {
        // Podés sobreescribir OnConnectedAsync / OnDisconnectedAsync para logging, grupos, etc.
    }
}
