using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetAllAsync(bool isEnabled, bool isDeleted);

        Task<Permission> GetByIdAsync(int id);

        Task<IEnumerable<RolePermission>> GetAllRolePermisionAsync(bool isEnabled, bool isDeleted);

        Task<IEnumerable<UserPermission>> GetAllUserPermisionAsync(bool isEnabled, bool isDeleted);

        Task<RolePermission> GetRolePermissionByIdAsync(int id);

        Task<UserPermission> GetUserPermissionByIdAsync(int id);

        Task<bool> UserHasPermissionAsync(string userId, string permission);
        Task<PermissionResponse> GetAllUserPermisionFromUserAsync(string userId);
    }
}
