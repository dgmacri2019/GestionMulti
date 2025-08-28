using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRSalesNotifier : ISalesNotifier
    {//private readonly IHubContext<ClientsHub, IClientsClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRSalesNotifier(/*IHubContext<ClientsHub, IClientsClient> hub*/INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
        }

        public async Task NotifyAsync(int saleId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            VentaChangeNotification notification = accion switch
            {
                ChangeType.Created => new SaleCreado(saleId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new SaleActualizado(saleId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new SaleEliminado(saleId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ClientesActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new SaleChangedItem(notification));
        }
    }
}
