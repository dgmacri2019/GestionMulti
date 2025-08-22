using GestionComercial.API.Hubs;
using GestionComercial.Applications.Notifications;
using Microsoft.AspNetCore.SignalR;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ArticleChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRArticlesNotifier : IArticlesNotifier
    {
        private readonly IHubContext<ArticlesHub, IArticlesClient> _hub;

        public SignalRArticlesNotifier(IHubContext<ArticlesHub, IArticlesClient> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAsync(int articleId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ArticuloChangeNotification notification = accion switch
            {
                ChangeType.Created => new ArticleCreado(articleId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Updated => new ArticleActualizado(articleId, DateTimeOffset.UtcNow, nombre),
                ChangeType.Deleted => new ArticleEliminado(articleId, DateTimeOffset.UtcNow),
                _ => throw new ArgumentException("Acción inválida")
            };
            await _hub.Clients.All.ArticulosActualizados(notification);
        }
    }
}
