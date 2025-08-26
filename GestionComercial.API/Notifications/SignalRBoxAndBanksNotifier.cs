using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.BoxAndBankChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRBoxAndBanksNotifier : IBoxAndBanksNotifier
    {
        //private readonly IHubContext<BoxAndBanksHub, IBoxAndBanksClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRBoxAndBanksNotifier(/*IHubContext<BoxAndBanksHub, IBoxAndBanksClient> hub*/INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
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
            //await _hub.Clients.All.CajasYBancosActualizados(notification);

            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new BoxAndBankChangedItem(notification));
        }
    }
}
