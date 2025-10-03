using GestionComercial.API.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

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
        private readonly IHubContext<InvoiceHub, IInvoicesClient> _invoicesHub;
        private readonly IHubContext<GeneralParametersHub, IParametersClient> _parametersHub;
        private readonly IHubContext<MasterClassHub, IMasterClassClient> _masterClassHub;
        private readonly IHubContext<UsersHub, IUsersClient> _userHub;
        // inyectá aquí también otros hubs si los vas a usar

        public NotificationDispatcher(INotificationQueue queue, ILogger<NotificationDispatcher> logger, IHubContext<ArticlesHub, IArticlesClient> articlesHub,
            IHubContext<BankParametersHub, IBankParametersClient> bankParametersHub, IHubContext<BoxAndBanksHub, IBoxAndBanksClient> boxAndBankHub,
            IHubContext<ClientsHub, IClientsClient> clientsHub, IHubContext<ProvidersHub, IProvidersClient> providersHub,
            IHubContext<SalesHub, ISalesClient> salesHub, IHubContext<GeneralParametersHub, IParametersClient> parametersHub,
            IHubContext<MasterClassHub, IMasterClassClient> masterClassHub, IHubContext<UsersHub, IUsersClient> userHub,
            IHubContext<InvoiceHub, IInvoicesClient> invoicesHub)
        {
            _queue = queue;
            _logger = logger;
            _articlesHub = articlesHub;
            _bankParametersHub = bankParametersHub;
            _boxAndBankHub = boxAndBankHub;
            _clientsHub = clientsHub;
            _providersHub = providersHub;
            _salesHub = salesHub;
            _parametersHub = parametersHub;
            _masterClassHub = masterClassHub;
            _userHub = userHub;
            _invoicesHub = invoicesHub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationDispatcher started");

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
                        case UserChangedItem p:
                            await _userHub.Clients.All.UsuariosActualizados(p.Notification);
                            break;
                        case ClientChangedItem a:
                            await _clientsHub.Clients.All.ClientesActualizados(a.Notification);
                            break;
                        case ProviderChangedItem a:
                            await _providersHub.Clients.All.ProveedoresActualizados(a.Notification);
                            break;
                        case SaleChangedItem s:
                            try
                            {
                                _logger.LogInformation("Dispatching SaleChangedItem. SaleId={SaleId}", s.Notification?.SaleId ?? 0);
                                // debug JSON serializado (útil para ver si serializa correctamente)
                                try
                                {
                                    var json = JsonSerializer.Serialize(s.Notification);
                                    _logger.LogDebug("VentaChangeNotification JSON: {Json}", json);
                                }
                                catch (Exception exJson)
                                {
                                    _logger.LogWarning(exJson, "No se pudo serializar VentaChangeNotification para debug");
                                }

                                await _salesHub.Clients.All.VentasActualizados(s.Notification);
                                _logger.LogInformation("VentasActualizados invoked (typed) for SaleId={SaleId}", s.Notification?.SaleId ?? 0);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Typed VentasActualizados failed for SaleId={SaleId}, trying fallback SendAsync", s.Notification?.SaleId ?? 0);
                                try
                                {
                                    await _salesHub.Clients.All.VentasActualizados(s.Notification);
                                    _logger.LogInformation("Fallback SendAsync succeeded for VentasActualizados SaleId={SaleId}", s.Notification?.SaleId ?? 0);
                                }
                                catch (Exception ex2)
                                {
                                    _logger.LogError(ex2, "Fallback SendAsync ALSO failed for VentasActualizados SaleId={SaleId}", s.Notification?.SaleId ?? 0);
                                }
                            }
                            break;
                        case InvoiceChangedItem s:
                            try
                            {
                                _logger.LogInformation("Dispatching InvoiceChangedItem. InvoiceId={InvoiceId}", s.Notification?.InvoiceId ?? 0);
                                // debug JSON serializado (útil para ver si serializa correctamente)
                                try
                                {
                                    var json = JsonSerializer.Serialize(s.Notification);
                                    _logger.LogDebug("FacturaChangeNotification JSON: {Json}", json);
                                }
                                catch (Exception exJson)
                                {
                                    _logger.LogWarning(exJson, "No se pudo serializar FacturaChangeNotification para debug");
                                }

                                await _invoicesHub.Clients.All.FacturasActualizados(s.Notification);
                                _logger.LogInformation("FacturasActualizados invoked (typed) for InvoiceId={InvoiceId}", s.Notification?.InvoiceId ?? 0);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Typed VentasActualizados failed for InvoiceId={InvoiceId}, trying fallback SendAsync", s.Notification?.InvoiceId ?? 0);
                                try
                                {
                                    await _invoicesHub.Clients.All.FacturasActualizados(s.Notification);
                                    _logger.LogInformation("Fallback SendAsync succeeded for FacturasActualizados InvoiceId={InvoiceId}", s.Notification?.InvoiceId ?? 0);
                                }
                                catch (Exception ex2)
                                {
                                    _logger.LogError(ex2, "Fallback SendAsync ALSO failed for VentasActualizados InvoiceId={InvoiceId}", s.Notification?.InvoiceId ?? 0);
                                }
                            }
                            break;
                        case GeneralParameterChangedItem s:
                            await _parametersHub.Clients.All.ParametrosGeneralesActualizados(s.Notification);
                            break;
                        case MasterClassChangedItem s:
                            try
                            {
                                _logger.LogInformation("Dispatching MasterClassChangedItem", s.Notification?.Id ?? 0);
                                // debug JSON serializado (útil para ver si serializa correctamente)
                                try
                                {
                                    var json = JsonSerializer.Serialize(s.Notification);
                                    _logger.LogDebug("MasterClassChangeNotification JSON: {Json}", json);
                                }
                                catch (Exception exJson)
                                {
                                    _logger.LogWarning(exJson, "No se pudo serializar MasterClassChangeNotification para debug");
                                }

                                await _masterClassHub.Clients.All.ClaseMaestraActualizados(s.Notification);
                                _logger.LogInformation("MasterClassActualizados invoked (typed) ", s.Notification?.Id ?? 0);
                            }
                            catch (Exception ex)
                            {

                                throw;
                            }
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

            _logger.LogInformation("NotificationDispatcher stopping");

        }
    }
}
