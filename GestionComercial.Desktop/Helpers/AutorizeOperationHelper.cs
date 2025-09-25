using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters.Security;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Helpers
{
    internal class AutorizeOperationHelper
    {
        public static bool ValidateModule(ModuleType moduleType)
        {
            return LoginUserCache.Permisions.Count(p => p.ModuleType == moduleType) > 0;            
        }

        public static bool ValidateOperation(ModuleType moduleType, string operationType)
        {
            List<Permission> permissions = LoginUserCache.Permisions;
            if (permissions.Count() == 0)
                return false;
            Permission permission = permissions
                .First(p => p.ModuleType == moduleType && p.Name == operationType);
            if (permission == null)
                return false;
            UserPermission userPermission = permission.UserPermissions
                .First(up => up.PermissionId == permission.Id && up.UserId == LoginUserCache.UserId);

            return userPermission != null && userPermission.IsEnabled;
        }
    }
}
