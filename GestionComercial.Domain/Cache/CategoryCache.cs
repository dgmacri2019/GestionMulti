using GestionComercial.Domain.Entities.Stock;

namespace GestionComercial.Domain.Cache
{
    public class CategoryCache : ICache
    {
        private static CategoryCache? _instance;
        public static CategoryCache Instance => _instance ??= new CategoryCache();

        private List<Category> _categories;
        public static bool Reading { get; set; } = false;

        public bool HasData => _categories != null && _categories.Any() && !Reading;

        public CategoryCache()
        {
            CacheManager.Register(this);
        }


        public List<Category> GetAll()
        {
            return _categories.OrderBy(c => c.Description).ToList();
        }
        public List<Category> Search(string name, bool isEnabled, bool isDeleted)
        {
            try
            {
                return _categories != null ? _categories
                              .Where(p => p.IsEnabled == isEnabled
                                       && p.IsDeleted == isDeleted)
                              .OrderBy(c => c.Description)
                              .ToList()
                              :
                              [];
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Set(List<Category> categories)
        {
            try
            {
                _categories = categories;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Set(Category client)
        {
            try
            {
                _categories.Add(client);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Update(Category client)
        {
            try
            {
                Category? Category = _categories.FirstOrDefault(c => c.Id == client.Id);
                if (Category != null)
                {
                    _categories.Remove(Category);
                    _categories.Add(client);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Remove(Category client)
        {
            try
            {
                Category? Category = _categories.FirstOrDefault(c => c.Id == client.Id);
                if (Category != null)
                    _categories.Remove(Category);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public Category? FindById(int id)
        {
            try
            {
                return _categories != null ?
                                _categories.FirstOrDefault(c => c.Id == id)
                               :
                               null;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void ClearCache()
        {
            _categories?.Clear();
        }
    }
}
