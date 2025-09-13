using GestionComercial.Domain.DTOs.Stock;

namespace GestionComercial.Domain.Cache
{
    public class CategoryCache : ICache
    {
        private static CategoryCache? _instance;
        public static CategoryCache Instance => _instance ??= new CategoryCache();

        private List<CategoryViewModel> _categories;
        public static bool Reading { get; set; } = false;

        public bool HasData => _categories != null && _categories.Any() && !Reading;

        public CategoryCache()
        {
            CacheManager.Register(this);
        }


        public List<CategoryViewModel> GetAll()
        {
            return _categories.OrderBy(c => c.Description).ToList();
        }
        public List<CategoryViewModel> Search(string name, bool isEnabled, bool isDeleted)
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
        public void Set(List<CategoryViewModel> categories)
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
        public void Set(CategoryViewModel client)
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
        public void Update(CategoryViewModel client)
        {
            try
            {
                CategoryViewModel? Category = _categories.FirstOrDefault(c => c.Id == client.Id);
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
        public void Remove(CategoryViewModel client)
        {
            try
            {
                CategoryViewModel? Category = _categories.FirstOrDefault(c => c.Id == client.Id);
                if (Category != null)
                    _categories.Remove(Category);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public CategoryViewModel? FindById(int id)
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
