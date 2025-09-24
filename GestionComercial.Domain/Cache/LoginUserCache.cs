using GestionComercial.Domain.Entities.Masters.Security;

namespace GestionComercial.Domain.Cache
{
    public class LoginUserCache : ICache
    {
        public static string AuthToken { get; set; }
        public static string Password { get; set; }
        public static string UserName { get; set; }
        public static string UserRole { get; set; }
        public static string UserId { get; set; }
        public static List<Permission> Permisions { get; set; }

        public void ClearCache()
        {
            AuthToken = string.Empty;
            Password = string.Empty;
            UserName = string.Empty;
            UserRole = string.Empty;
            UserId = string.Empty;
            Permisions.Clear();
        }
    }
}
