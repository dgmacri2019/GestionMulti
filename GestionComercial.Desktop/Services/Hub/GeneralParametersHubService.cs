using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using static GestionComercial.Domain.Notifications.GeneralParameterChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class GeneralParametersHubService : IHub
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ParametroGeneralChangeNotification> ParametroGeneralCambiado;

        public GeneralParametersHubService(string hubUrl)
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
                Debug.WriteLine("[GeneralParametersHub] Reconnecting: " + ex?.Message);
                return Task.CompletedTask;
            };

            _connection.Reconnected += id =>
            {
                Debug.WriteLine("[GeneralParametersHub] Reconnected: " + id);
                return Task.CompletedTask;
            };

            _connection.Closed += ex =>
            {
                Debug.WriteLine("[GeneralParametersHub] Closed. Ex: " + ex?.Message);
                return Task.CompletedTask;
            };
            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ParametroGeneralChangeNotification>("ParametrosGeneralesActualizados", (notification) =>
            {
                Debug.WriteLine("[GeneralParametersHub] ParametrosGeneralesActualizados received: " + (notification == null ? "NULL" : notification.ToString()));
                try
                {
                    ParametroGeneralCambiado?.Invoke(notification);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[GeneralParametersHub] Handler ParametroGeneralCambiado threw: " + ex);
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
                    Debug.WriteLine("[GeneralParametersHub] Connected");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("[GeneralParametersHub] StartAsync failed: " + ex);
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
