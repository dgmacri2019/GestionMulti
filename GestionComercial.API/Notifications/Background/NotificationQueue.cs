using System.Threading.Channels;

namespace GestionComercial.API.Notifications.Background
{
    public interface INotificationItem { } // marcador

    public interface INotificationQueue
    {
        ValueTask EnqueueAsync(INotificationItem item, CancellationToken ct = default);
        IAsyncEnumerable<INotificationItem> DequeueAllAsync(CancellationToken ct);
    }

    public class NotificationQueue : INotificationQueue
    {
        private readonly Channel<INotificationItem> _channel =
            Channel.CreateUnbounded<INotificationItem>(new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false,
                AllowSynchronousContinuations = false
            });

        public ValueTask EnqueueAsync(INotificationItem item, CancellationToken ct = default)
            => _channel.Writer.WriteAsync(item, ct);

        public IAsyncEnumerable<INotificationItem> DequeueAllAsync(CancellationToken ct)
            => _channel.Reader.ReadAllAsync(ct);
    }
}
