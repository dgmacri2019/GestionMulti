using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRSalesNotifier : ISalesNotifier
    {
        private readonly INotificationQueue _queue;
        private readonly ILogger<SignalRSalesNotifier> _logger;
        public SignalRSalesNotifier(INotificationQueue queue, ILogger<SignalRSalesNotifier> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        public async Task NotifyAsync(int saleId, string nombre, ChangeType accion)
        {
            _logger.LogInformation("NotifyAsync called for saleId={SaleId} action={Action}", saleId, accion);
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            VentaChangeNotification notification = accion switch
            {
                ChangeType.Created => new SaleCreado(saleId, DateTimeOffset.UtcNow, nombre, accion),
                ChangeType.Updated => new SaleActualizado(saleId, DateTimeOffset.UtcNow, nombre, accion),
                ChangeType.Deleted => new SaleEliminado(saleId, DateTimeOffset.UtcNow, accion),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ClientesActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new SaleChangedItem(notification));
            _logger.LogDebug("SaleChangedItem enqueued for saleId={SaleId}", saleId);

        }
    }
}
