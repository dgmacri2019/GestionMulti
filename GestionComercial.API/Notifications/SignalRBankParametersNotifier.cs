using GestionComercial.API.Hubs;
using GestionComercial.Applications.Notifications;
using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.BankParameterChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRBankParametersNotifier : IBankParametersNotifier
    {
        private readonly IHubContext<BankParametersHub, IBankParametersClient> _hub;

        public SignalRBankParametersNotifier(IHubContext<BankParametersHub, IBankParametersClient> hub)
        {
            _hub = hub;
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
            await _hub.Clients.All.ParametrosBancariosActualizados(notification);
        }
    }
}
