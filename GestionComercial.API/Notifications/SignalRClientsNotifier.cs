using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Api.Notifications
{
    public class SignalRClientsNotifier : IClientsNotifier
    {
        //private readonly IHubContext<ClientsHub, IClientsClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRClientsNotifier(/*IHubContext<ClientsHub, IClientsClient> hub*/INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
        }

        public async Task NotifyAsync(int clienteId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ClienteChangeNotification notification = accion switch
            {
                ChangeType.Created => new ClientCreado(clienteId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new ClientActualizado(clienteId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new ClientEliminado(clienteId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ClientesActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new ClientChangedItem(notification));
        }
    }
}
