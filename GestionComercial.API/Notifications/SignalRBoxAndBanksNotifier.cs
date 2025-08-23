using GestionComercial.API.Hubs;
using GestionComercial.Applications.Notifications;
using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.BoxAndBankChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRBoxAndBanksNotifier : IBoxAndBanksNotifier
    {
        private readonly IHubContext<BoxAndBanksHub, IBoxAndBanksClient> _hub;

        public SignalRBoxAndBanksNotifier(IHubContext<BoxAndBanksHub, IBoxAndBanksClient> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAsync(int clienteId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            CajaYBancoChangeNotification notification = accion switch
            {
                ChangeType.Created => new BoxAndBankCreado(clienteId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new BoxAndBankActualizado(clienteId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new BoxAndBankEliminado(clienteId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            await _hub.Clients.All.CajasYBancosActualizados(notification);
        }
    }
}
