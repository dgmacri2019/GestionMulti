using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.BankParameterChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class BankParametersHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ParametroBancarioChangeNotification> ParametrosBancariosCambiado;

        public BankParametersHubService(string hubUrl)
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
                Debug.WriteLine("[BankParametersHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[BankParametersHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[BankParametersHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };
            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ParametroBancarioChangeNotification>("ParametrosBancariosActualizados", (notification) =>
            {
                Debug.WriteLine("[BankParametersHub] ParametrosBancariosActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    ParametrosBancariosCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[BankParametersHub] Handler ParametrosBancarioCambiado threw: " + ex);
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
                    Debug.WriteLine("[BankParametersHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[BankParametersHub] StartAsync failed: " + ex);
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
