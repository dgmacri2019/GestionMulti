using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;
using static GestionComercial.Domain.Notifications.UserChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRUsersNotifier : IUsersNotifier
    {
        private readonly INotificationQueue _queue;
        private readonly ILogger<SignalRUsersNotifier> _logger;

        public SignalRUsersNotifier(INotificationQueue queue, ILogger<SignalRUsersNotifier> logger)
        {
            _queue = queue;
            _logger = logger;

        }

        public async Task NotifyAsync(string userId, string nombre, ChangeType accion)
        {
            _logger.LogInformation("NotifyAsync called for userId={userId} action={action}", userId, accion);
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            UsuarioChangeNotification notification = accion switch
            {
                ChangeType.Created => new UserCreado(userId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new UserActualizado(userId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new UserEliminado(userId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ClientesActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new UserChangedItem(notification));
            _logger.LogDebug("UserChangedItem enqueued for userId={userId}", userId);

        }
    }
}
