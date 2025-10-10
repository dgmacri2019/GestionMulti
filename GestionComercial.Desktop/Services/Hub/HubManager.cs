namespace GestionComercial.Desktop.Services.Hub
{
    public static class HubManager
    {
        private static readonly List<IHub> _hubs = [];

        public static void Register(IHub hub)
        {
            _hubs.Add(hub);
        }

        public static async Task<Task> CloseAll()
        {
            foreach (var hub in _hubs)
            {
                await hub.StopAsync();
            }
            return Task.CompletedTask;
        }

    }
}
