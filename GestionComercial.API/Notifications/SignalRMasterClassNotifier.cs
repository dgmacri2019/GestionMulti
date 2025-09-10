using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRMasterClassNotifier : IMasterClassNotifier
    {
        private readonly INotificationQueue _queue;
        public SignalRMasterClassNotifier(INotificationQueue queue)
        {
            _queue = queue;
        }

        public async Task NotifyAsync(int id, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ClaseMaestraChangeNotification notification = accion switch
            {
                ChangeType.Created => new MasterClassCreado(id, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new MasterClassActualizado(id, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new MasterClassEliminado(id, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new MasterClassChangedItem(notification));
        }
    }
}
