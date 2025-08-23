namespace GestionComercial.Desktop.Cache
{
    public static class CacheManager
    {
        private static readonly List<ICache> _caches = [];

        public static void Register(ICache cache)
        {
            _caches.Add(cache);
        }

        public static void ClearAll()
        {
            foreach (var cache in _caches)
            {
                cache.ClearCache();
            }
        }
    }
}
