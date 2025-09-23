using Microsoft.AspNetCore.SignalR.Client;
using static GestionComercial.Domain.Notifications.UserChangeNotification;

namespace GestionComercial.Desktop.Services.Hub
{
    internal class UsersHubService
    {
        private readonly HubConnection _connection;

        // Evento que levantamos cuando llega una notificación
        public event Action<UsuarioChangeNotification> UsuarioCambiado;

        public UsersHubService(string hubUrl)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(hubUrl)
                .WithAutomaticReconnect()
                //.WithServerTimeout(new TimeSpan(100))
                .Build();

            // Aquí registramos el método que el servidor va a invocar
            _connection.On<UsuarioChangeNotification>("UsuariosActualizados", (notification) =>
            {
                UsuarioCambiado?.Invoke(notification);
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
