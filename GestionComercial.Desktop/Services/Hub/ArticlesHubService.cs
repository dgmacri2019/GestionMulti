using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.ArticleChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class ArticlesHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ArticuloChangeNotification> ArticuloCambiado;

        public ArticlesHubService(string hubUrl)
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
                Debug.WriteLine("[ArticlesHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[ArticlesHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[ArticlesHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };
            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ArticuloChangeNotification>("ArticulosActualizados", (notification) =>
            {
                Debug.WriteLine("[ArticlesHub] ArticulosActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    ArticuloCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ArticlesHub] Handler ArticuloCambiado threw: " + ex);
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
                    Debug.WriteLine("[ArticlesHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[ArticlesHub] StartAsync failed: " + ex);
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
