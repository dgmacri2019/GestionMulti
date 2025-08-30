using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Notifications
{
    public interface IParametersNotifier
    {
        Task NotifyAsync(int paramaterId, string description, ChangeType action);
    }
}
