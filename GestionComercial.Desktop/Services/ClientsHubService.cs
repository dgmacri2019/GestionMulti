using Microsoft.AspNetCore.SignalR.Client;
using static GestionComercial.Domain.Notifications.ClientChangeNotification;

namespace GestionComercial.Desktop.Services
{
    internal class ClientsHubService
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<ClienteChangeNotification> ClienteCambiado;

        public ClientsHubService(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl) // ejemplo: "https://localhost:5001/hubs/clientes"
                .WithAutomaticReconnect()
                .Build();

            // Aquí registramos el método que el servidor va a invocar
            _connection.On<ClienteChangeNotification>("ClientesActualizados", (notification) =>
            {
                ClienteCambiado?.Invoke(notification);
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
