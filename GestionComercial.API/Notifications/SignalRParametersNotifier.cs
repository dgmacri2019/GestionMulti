using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.GeneralParameterChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRParametersNotifier : IParametersNotifier
    {//private readonly IHubContext<ClientsHub, IClientsClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRParametersNotifier(INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
        }

        public async Task NotifyAsync(int generalParameterId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ParametroGeneralChangeNotification notification = accion switch
            {
                ChangeType.Created => new GeneralParameterCreado(generalParameterId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new GeneralParameterActualizado(generalParameterId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new GeneralParameterEliminado(generalParameterId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ClientesActualizados(notification);
            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new GeneralParameterChangedItem(notification));
        }
    }
}
