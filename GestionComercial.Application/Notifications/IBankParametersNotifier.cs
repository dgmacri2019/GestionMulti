using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IBankParametersNotifier
    {
        Task NotifyAsync(int Id, string Name, ChangeType action);
    }
}
