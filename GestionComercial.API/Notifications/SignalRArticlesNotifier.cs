using GestionComercial.API.Notifications.Background;
using GestionComercial.Applications.Notifications;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.ArticleChangeNotification;

namespace GestionComercial.API.Notifications
{
    public class SignalRArticlesNotifier : IArticlesNotifier
    {
        //private readonly IHubContext<ArticlesHub, IArticlesClient> _hub;
        private readonly INotificationQueue _queue;
        public SignalRArticlesNotifier(/*IHubContext<ArticlesHub, IArticlesClient> hub*/INotificationQueue queue)
        {
            _queue = queue;
            //_hub = hub;
        }

        public async Task NotifyAsync(int articleId, string nombre, ChangeType accion)
        {
            // Difunde a todas las terminales. Si querés segmentar por sucursal, usá Groups.
            ArticuloChangeNotification notification = accion switch
            {
                ChangeType.Created => new ArticleCreado(articleId, DateTimeOffset.UtcNow, nombre, accion),
                ChangeType.Updated => new ArticleActualizado(articleId, DateTimeOffset.UtcNow, nombre, accion),
                ChangeType.Deleted => new ArticleEliminado(articleId, DateTimeOffset.UtcNow, accion),
                _ => throw new ArgumentException("Acción inválida")
            };
            //await _hub.Clients.All.ArticulosActualizados(notification);

            // 👉 encolamos (rápido) y devolvemos el control al request
            await _queue.EnqueueAsync(new ArticleChangedItem(notification));
        }
    }
}
