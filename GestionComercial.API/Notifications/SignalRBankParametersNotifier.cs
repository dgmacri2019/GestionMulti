using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.BankParameterChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRBankParametersNotifier : IBankParametersNotifier
    {
        //private readonly IHubContext<BankParametersHub, IBankParametersClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRBankParametersNotifier(/*IHubContext<BankParametersHub, IBankParametersClient> hub*/INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
        }

        public async Task NotifyAsync(int Id, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ParametroBancarioChangeNotification notification = accion switch
            {
                ChangeType.Created => new BankParameterCreado(Id, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new BankParameterActualizado(Id, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new BankParameterEliminado(Id, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ParametrosBancariosActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new BankParameterChangedItem(notification));
        }
    }
}
