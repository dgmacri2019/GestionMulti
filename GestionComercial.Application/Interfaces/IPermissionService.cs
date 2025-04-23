using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IPermissionService
    {
        Task<GeneralResponse> AddAsync(Permission permission);
        GeneralResponse Add(Permission permission);


        Task<GeneralResponse> UpdateAsync(Permission permission);
        GeneralResponse Update(Permission permission);


        Task<GeneralResponse> DeleteAsync(Permission permission);
        GeneralResponse Delete(Permission permission);


        Task<IEnumerable<Permission>> GetAllAsync();
        IEnumerable<Permission> GetAll();


        Task<Permission> GetByIdAsync(int id);
        Permission GetById(int id);


        Task<GeneralResponse> AddRolePermissionAsync(RolePermission rolePermission);
        GeneralResponse AddRolePermission(RolePermission rolePermission);

        Task<GeneralResponse> UpdateRolePermissionAsync(RolePermission rolePermission);
        GeneralResponse UpdateRolePermission(RolePermission rolePermission);

        Task<GeneralResponse> DeleteRolePermissionAsync(RolePermission rolePermission);
        GeneralResponse DeleteRolePermission(RolePermission rolePermission);


        Task<GeneralResponse> AddUserPermissionAsync(UserPermission userPermission);
        GeneralResponse AddUserPermission(UserPermission userPermission);


        Task<GeneralResponse> UpdateUserPermissionAsync(UserPermission userPermission);
        GeneralResponse UpdateUserPermission(UserPermission userPermission);


        Task<GeneralResponse> DeleteUserPermissionAsync(UserPermission userPermission);
        GeneralResponse DeleteUserPermission(UserPermission userPermission);


        Task<IEnumerable<RolePermission>> GetAllRolePermisionAsync();
        IEnumerable<RolePermission> GetAllRolePermision();


        Task<IEnumerable<UserPermission>> GetAllUserPermisionAsync();
        IEnumerable<UserPermission> GetAllUserPermision();


        Task<RolePermission> GetRolePermissionByIdAsync(int id);
        RolePermission GetRolePermissionById(int id);


        Task<UserPermission> GetUserPermissionByIdAsync(int id);
        UserPermission GetUserPermissionById(int id);



        Task<bool> UserHasPermissionAsync(string userId, string permission);
    }
}
