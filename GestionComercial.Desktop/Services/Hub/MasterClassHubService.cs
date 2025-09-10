using Microsoft.AspNetCore.SignalR.Client;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class MasterClassHubService
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ClaseMaestraChangeNotification> ClaseMaestraCambiado;

        public MasterClassHubService(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
               .WithUrl(hubUrl)
               .WithAutomaticReconnect()
               .Build();

            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ClaseMaestraChangeNotification>("ClaseMaestraActualizados", (notification) =>
            {
                ClaseMaestraCambiado?.Invoke(notification);
            });

        }

        public async Task StartAsync()
        {
            await _connection.StartAsync();
        }

        public async Task StopAsync()
        {
            await _connection.StopAsync();
        }
    }
}
