using Microsoft.AspNetCore.SignalR.Client;
using static GestionComercial.Domain.Notifications.SaleChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class SalesHubService
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<VentaChangeNotification> VentaCambiado;

        public SalesHubService(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                //.WithServerTimeout(new TimeSpan(100))
                .Build();

            // Aquí registramos el método que el servidor va a invocar
            _connection.On<VentaChangeNotification>("VentassActualizados", (notification) =>
            {
                VentaCambiado?.Invoke(notification);
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
