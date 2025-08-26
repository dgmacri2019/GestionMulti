using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ProviderChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRProvidersNotifier : IProvidersNotifier
    {
        //private readonly IHubContext<ProvidersHub, IProvidersClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRProvidersNotifier(/*IHubContext<ProvidersHub, IProvidersClient> hub*/INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
        }

        public async Task NotifyAsync(int providerId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ProveedorChangeNotification notification = accion switch
            {
                ChangeType.Created => new ProviderCreado(providerId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new ProviderActualizado(providerId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new ProviderEliminado(providerId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ProveedoresActualizados(notification);

            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new ProviderChangedItem(notification));
        }
    }
}
