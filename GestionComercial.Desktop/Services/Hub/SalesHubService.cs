using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class SalesHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<VentaChangeNotification> VentaCambiado;
        public event Action<string>? SalesChanged;

        public SalesHubService(string hubUrl)
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
                Debug.WriteLine("[SalesHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[SalesHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[SalesHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.On<VentaChangeNotification>("VentasActualizados", (notification) =>
            {
                Debug.WriteLine("[SalesHub] VentasActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    VentaCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[SalesHub] Handler VentaCambiado threw: " + ex);
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
                    Debug.WriteLine("[SalesHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[SalesHub] StartAsync failed: " + ex);
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
