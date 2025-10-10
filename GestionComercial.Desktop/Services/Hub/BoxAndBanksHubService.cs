using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.BoxAndBankChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class BoxAndBanksHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<CajaYBancoChangeNotification> CajaYBancoCambiado;

        public BoxAndBanksHubService(string hubUrl)
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
                Debug.WriteLine("[BoxBanksHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[BoxBanksHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[BoxBanksHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };

            // Aquí registramos el método que el servidor va a invocar
            _connection.On<CajaYBancoChangeNotification>("CajasYBancosActualizados", (notification) =>
            {
                Debug.WriteLine("[BoxBanksHub] CajasYBancosActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    CajaYBancoCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[BoxBanksHub] Handler CajaYBancoCambiado threw: " + ex);
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
                    Debug.WriteLine("[BoxBanksHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[BoxBanksHub] StartAsync failed: " + ex);
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
