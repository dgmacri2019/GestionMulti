using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.ProviderChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class ProvidersHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ProveedorChangeNotification> ProveedorCambiado;

        public ProvidersHubService(string hubUrl)
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
                Debug.WriteLine("[ProvidersHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[ProvidersHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[ProvidersHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };
            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ProveedorChangeNotification>("ProveedoresActualizados", (notification) =>
            {
                Debug.WriteLine("[ProvidersHub] ProveedoresActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    ProveedorCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ProvidersHub] Handler ProveedorCambiado threw: " + ex);
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
                    Debug.WriteLine("[ProvidersHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ProvidersHub] StartAsync failed: " + ex);
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
