using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.UserChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class UsersHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<UsuarioChangeNotification> UsuarioCambiado;

        public UsersHubService(string hubUrl)
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
                Debug.WriteLine("[UsersHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[UsersHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[UsersHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };
            // Aquí registramos el método que el servidor va a invocar
            _connection.On<UsuarioChangeNotification>("UsuariosActualizados", (notification) =>
            {
                Debug.WriteLine("[UsersHub] UsuariosActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    UsuarioCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[UsersHub] Handler UsuarioCambiado threw: " + ex);
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
                    Debug.WriteLine("[UsersHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[UsersHub] StartAsync failed: " + ex);
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
