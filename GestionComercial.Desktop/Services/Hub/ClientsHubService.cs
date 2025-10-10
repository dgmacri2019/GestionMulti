using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class ClientsHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ClienteChangeNotification> ClienteCambiado;

        public ClientsHubService(string hubUrl)
        {
            HubManager.Register(this);
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                 .ConfigureLogging(logging =>
                 {
                     logging.SetMinimumLevel(LogLevel.Debug);
                     logging.AddDebug();
                 })
                .Build();
            _connection.Reconnecting += ex =>
            {
                Debug.WriteLine("[ClientsHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[ClientsHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[ClientsHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };
            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ClienteChangeNotification>("ClientesActualizados", (notification) =>
            {
                Debug.WriteLine("[ClientsHub] ClientesActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    ClienteCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ClientsHub] Handler ClienteCambiado threw: " + ex);
                }
            });
        }

        public async Task StartAsync()
        {
            if (_connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    await _connection.StartAsync();
                    Debug.WriteLine("[ClientsHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ClientsHub] StartAsync failed: " + ex);
                    throw;
                }
            }
        }

        public async Task StopAsync()
        {
            await _connection.StopAsync();
        }
    }
}
