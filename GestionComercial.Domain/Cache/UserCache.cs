using GestionComercial.Domain.DTOs.User;

namespace GestionComercial.Domain.Cache
{
    public class UserCache : ICache
    {
        private static UserCache _instance;
        public static UserCache Instance => _instance ??= new UserCache();
        private List<UserRoleDto> UserRoleDtos =
       [
           new UserRoleDto { Id = 0, Name = "Seleccione el Rol" },
            new UserRoleDto { Id = 1, Name = "Developer" },
            new UserRoleDto { Id = 2, Name = "Administrador" },
            new UserRoleDto { Id = 3, Name = "Supervisor" },
            new UserRoleDto { Id = 4, Name = "Operador"},
            new UserRoleDto { Id = 5, Name = "Cajero" }
       ];
        private List<UserViewModel?> _users;

        public static bool Reading { get; set; } = false;

        private UserCache()
        {
            CacheManager.Register(this);
        }

        public List<UserViewModel> GetAll()
        {
            return _users;
        }
        public List<UserViewModel?> Search(string name, bool isEnabled, bool isDeleted)
        {

            int userRoleId = UserRoleDtos.FirstOrDefault(r => r.Name == LoginUserCache.UserRole).Id;

            return _users != null ? _users
                .Where(a => a.RoleId >= userRoleId && a.IsEnabled == isEnabled && a.IsDeleted == isDeleted
                       && ((a.FullName?.ToLower().Contains(name.ToLower()) ?? false)
                        || (a.RoleName?.ToLower().Contains(name.ToLower()) ?? false)))
                .ToList()
                :
                [];
        }
        public void Set(List<UserViewModel?> users)
        {
            _users = users;
        }
        public void Set(UserViewModel? user)
        {
            try
            {
                _users.Add(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public UserViewModel? FindById(string id)
        {
            try
            {
                return _users != null ?
                                _users.FirstOrDefault(c => c.Id == id)
                               :
                               null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Update(UserViewModel? user)
        {
            try
            {
                UserViewModel? userViewModel = _users.FirstOrDefault(c => c.Id == user.Id);
                if (userViewModel != null)
                {
                    _users.Remove(userViewModel);
                    _users.Add(user);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void Remove(UserViewModel? user)
        {
            try
            {
                UserViewModel? userViewModel = _users.FirstOrDefault(c => c.Id == user.Id);
                if (userViewModel != null)
                    _users.Remove(userViewModel);
            }
            catch (Exception)
            {

                throw;
            }
        }



        public void ClearCache()
        {
            _users?.Clear();
        }
        public bool HasData => _users != null && _users.Any() && !Reading;
    }
}
