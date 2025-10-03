using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IInvoicesNotifier
    {
        Task NotifyAsync(int invoiceId, string clientName, ChangeType action);
    }
}
