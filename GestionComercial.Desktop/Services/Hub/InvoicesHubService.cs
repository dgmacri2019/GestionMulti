using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.InvoiceChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class InvoicesHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<FacturaChangeNotification> FacturaCambiado;
        public event Action<string>? InvoicesChanged;

        public InvoicesHubService(string hubUrl)
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
                Debug.WriteLine("[InvoicesHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[InvoicesHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[InvoicesHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.On<FacturaChangeNotification>("FacturasActualizados", (notification) =>
            {
                Debug.WriteLine("[InvoicesHub] FacturasActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    FacturaCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[InvoicesHub] Handler FacturaCambiado threw: " + ex);
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
                    Debug.WriteLine("[InvoicesHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[InvoicesHub] StartAsync failed: " + ex);
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
