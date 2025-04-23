using GestionComercial.Applications.Interfaces;

using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;


        public PermissionService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }


        public GeneralResponse Add(Permission permission)
        {
            _context.Permissions.Add(permission);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> AddAsync(Permission permission)
        {
            _context.Permissions.Add(permission);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse AddRolePermission(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> AddRolePermissionAsync(RolePermission rolePermission)
        {
            _context.RolePermissions.Add(rolePermission);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse AddUserPermission(UserPermission userPermission)
        {
            _context.UserPermissions.Add(userPermission);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> AddUserPermissionAsync(UserPermission userPermission)
        {
            _context.UserPermissions.Add(userPermission);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse Delete(Permission permission)
        {
            _context.Remove(permission);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> DeleteAsync(Permission permission)
        {
            _context.Remove(permission);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse DeleteRolePermission(RolePermission rolePermission)
        {
            _context.Remove(rolePermission);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> DeleteRolePermissionAsync(RolePermission rolePermission)
        {
            _context.Remove(rolePermission);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse DeleteUserPermission(UserPermission userPermission)
        {
            _context.Remove(userPermission);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> DeleteUserPermissionAsync(UserPermission userPermission)
        {
            _context.Remove(userPermission);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public IEnumerable<Permission> GetAll()
        {
            return _context.Permissions.ToList();
        }

        public async Task<IEnumerable<Permission>> GetAllAsync()
        {
            return await _context.Permissions.ToListAsync();
        }

        public IEnumerable<RolePermission> GetAllRolePermision()
        {
            return _context.RolePermissions.ToList();
        }

        public async Task<IEnumerable<RolePermission>> GetAllRolePermisionAsync()
        {
            return await _context.RolePermissions.ToListAsync();
        }

        public IEnumerable<UserPermission> GetAllUserPermision()
        {
            return _context.UserPermissions.ToList();
        }

        public async Task<IEnumerable<UserPermission>> GetAllUserPermisionAsync()
        {
            return await _context.UserPermissions.ToListAsync();
        }

        public Permission GetById(int id)
        {
            return _context.Permissions.Find(id);
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public RolePermission GetRolePermissionById(int id)
        {
            return _context.RolePermissions.Find(id);
        }

        public async Task<RolePermission> GetRolePermissionByIdAsync(int id)
        {
            return await _context.RolePermissions.FindAsync(id);
        }

        public UserPermission GetUserPermissionById(int id)
        {
            return _context.UserPermissions.Find(id);
        }

        public async Task<UserPermission> GetUserPermissionByIdAsync(int id)
        {
            return await _context.UserPermissions.FindAsync(id);
        }

        public GeneralResponse Update(Permission permission)
        {
            _context.Entry(permission).State = EntityState.Modified;
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> UpdateAsync(Permission permission)
        {
            _context.Entry(permission).State = EntityState.Modified;
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse UpdateRolePermission(RolePermission rolePermission)
        {
            _context.Entry(rolePermission).State = EntityState.Modified;
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> UpdateRolePermissionAsync(RolePermission rolePermission)
        {
            _context.Entry(rolePermission).State = EntityState.Modified;
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse UpdateUserPermission(UserPermission userPermission)
        {
            _context.Entry(userPermission).State = EntityState.Modified;
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> UpdateUserPermissionAsync(UserPermission userPermission)
        {
            _context.Entry(userPermission).State = EntityState.Modified;
            return await _dBHelper.SaveChangesAsync(_context);
        }


        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permission && up.IsEnabled && !up.IsDeleted);
        }
    }
}
