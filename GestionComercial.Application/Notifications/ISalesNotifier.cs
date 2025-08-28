using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface ISalesNotifier
    {
        Task NotifyAsync(int saleId, string clientName, ChangeType action);
    }
}
