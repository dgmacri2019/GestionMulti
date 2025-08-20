using GestionComercial.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class ClientsNotifier
    {
        private readonly IHubContext<ClientsHub, IClientsClient> _hub;

        public ClientsNotifier(IHubContext<ClientsHub, IClientsClient> hub)
        {
            _hub = hub;
        }

        public async Task NotificarCambio(int clienteId, string nombre, string tipo)
        {
            ClienteChangeNotification noti = tipo switch
            {
                "created" => new ClientCreado(clienteId, DateTimeOffset.UtcNow, nombre),
                "updated" => new ClientActualizado(clienteId, DateTimeOffset.UtcNow, nombre),
                "deleted" => new ClientEliminado(clienteId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Tipo inválido")
            };

            await _hub.Clients.All.ClientesActualizados(noti);
        }
    }
}
