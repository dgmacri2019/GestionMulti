using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.InvoiceChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRInvoicesNotifier : IInvoicesNotifier
    {
        private readonly INotificationQueue _queue;
        private readonly ILogger<SignalRInvoicesNotifier> _logger;

        public SignalRInvoicesNotifier(INotificationQueue queue, ILogger<SignalRInvoicesNotifier> logger)
        {
            _queue = queue;
            _logger = logger;
        }

        public async Task NotifyAsync(int invoiceId, string nombre, ChangeType accion)
        {
            _logger.LogInformation("NotifyAsync called for invoiceId={InvoiceId} action={Action}", invoiceId, accion);
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            FacturaChangeNotification notification = accion switch
            {
                ChangeType.Created => new InvoiceCreado(invoiceId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new InvoiceActualizado(invoiceId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new InvoiceEliminado(invoiceId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ClientesActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new InvoiceChangedItem(notification));
            _logger.LogDebug("SaleChangedItem enqueued for invoiceId={InvoiceId}", invoiceId);

        }
    }
}
