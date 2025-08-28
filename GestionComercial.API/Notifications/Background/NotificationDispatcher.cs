using GestionComercial.API.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GestionComercial.API.Notifications.Background
{
    public class NotificationDispatcher : BackgroundService
    {
        private readonly INotificationQueue _queue;
        private readonly ILogger<NotificationDispatcher> _logger;
        private readonly IHubContext<ArticlesHub, IArticlesClient> _articlesHub;
        private readonly IHubContext<BankParametersHub, IBankParametersClient> _bankParametersHub;
        private readonly IHubContext<BoxAndBanksHub, IBoxAndBanksClient> _boxAndBankHub;
        private readonly IHubContext<ClientsHub, IClientsClient> _clientsHub;
        private readonly IHubContext<ProvidersHub, IProvidersClient> _providersHub;
        private readonly IHubContext<SalesHub, ISalesClient> _salesHub;
        // inyectá aquí también otros hubs si los vas a usar

        public NotificationDispatcher(INotificationQueue queue, ILogger<NotificationDispatcher> logger, IHubContext<ArticlesHub, IArticlesClient> articlesHub,
            IHubContext<BankParametersHub, IBankParametersClient> bankParametersHub, IHubContext<BoxAndBanksHub, IBoxAndBanksClient> boxAndBankHub,
            IHubContext<ClientsHub, IClientsClient> clientsHub, IHubContext<ProvidersHub, IProvidersClient> providersHub, 
            IHubContext<SalesHub, ISalesClient> salesHub  )
        {
            _queue = queue;
            _logger = logger;
            _articlesHub = articlesHub;
            _bankParametersHub = bankParametersHub;
            _boxAndBankHub = boxAndBankHub;
            _clientsHub = clientsHub;
            _providersHub = providersHub;
            _salesHub = salesHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var item in _queue.DequeueAllAsync(stoppingToken))
            {
                try
                {
                    switch (item)
                    {
                        case ArticleChangedItem c:
                            await _articlesHub.Clients.All.ArticulosActualizados(c.Notification);
                            break;
                        case BankParameterChangedItem c:
                            await _bankParametersHub.Clients.All.ParametrosBancariosActualizados(c.Notification);
                            break;

                        case BoxAndBankChangedItem p:
                            await _boxAndBankHub.Clients.All.CajasYBancosActualizados(p.Notification);
                            break;

                        case ClientChangedItem a:
                            await _clientsHub.Clients.All.ClientesActualizados(a.Notification);
                            break;
                        case ProviderChangedItem a:
                            await _providersHub.Clients.All.ProveedoresActualizados(a.Notification);
                            break;
                        case SaleChangedItem s:
                            await _salesHub.Clients.All.VentasActualizados(s.Notification);
                            break;

                        default:
                            _logger.LogWarning("Notification item no reconocido: {Type}", item.GetType().Name);
                            break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error difundiendo notificación {Type}", item.GetType().Name);
                }
            }
        }
    }
}
