using GestionComercial.Domain.Notifications;

namespace GestionComercial.API.Notifications.Background
{
    public sealed record ArticleChangedItem(ArticleChangeNotification.ArticuloChangeNotification Notification) : INotificationItem;
    public sealed record BankParameterChangedItem(BankParameterChangeNotification.ParametroBancarioChangeNotification Notification) : INotificationItem;
    public sealed record BoxAndBankChangedItem(BoxAndBankChangeNotification.CajaYBancoChangeNotification Notification) : INotificationItem;
    public sealed record ClientChangedItem(ClientChangeNotification.ClienteChangeNotification Notification) : INotificationItem;
    public sealed record ProviderChangedItem(ProviderChangeNotification.ProveedorChangeNotification Notification) : INotificationItem;

}
