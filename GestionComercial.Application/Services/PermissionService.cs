using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Masters.Security;
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


        public async Task<IEnumerable<Permission>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.Permissions.Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted).ToListAsync();
        }

        public async Task<IEnumerable<RolePermission>> GetAllRolePermisionAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.RolePermissions.Where(rp => rp.IsEnabled == isEnabled && rp.IsDeleted == isDeleted).ToListAsync();
        }

        public async Task<IEnumerable<UserPermission>> GetAllUserPermisionAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.UserPermissions.Where(up => up.IsEnabled == isEnabled && up.IsDeleted == isDeleted).ToListAsync();
        }

        public async Task<Permission> GetByIdAsync(int id)
        {
            return await _context.Permissions.FindAsync(id);
        }

        public async Task<RolePermission> GetRolePermissionByIdAsync(int id)
        {
            return await _context.RolePermissions.FindAsync(id);
        }

        public async Task<UserPermission> GetUserPermissionByIdAsync(int id)
        {
            return await _context.UserPermissions.FindAsync(id);
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permission && up.IsEnabled && !up.IsDeleted);
        }
    }
}
