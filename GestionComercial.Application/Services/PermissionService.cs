using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
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


        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _context.Permissions
                .AsNoTracking()
                .Include(p => p.UserPermissions)
                .Include(p => p.RolePermissions)
                .ToListAsync();
        }

        public async Task<IEnumerable<RolePermission>> GetAllRolePermisionAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.RolePermissions.AsNoTracking().Where(rp => rp.IsEnabled == isEnabled && rp.IsDeleted == isDeleted).ToListAsync();
        }

        public async Task<IEnumerable<UserPermission>> GetAllUserPermisionAsync(bool isEnabled, bool isDeleted)
        {
            return await _context.UserPermissions.AsNoTracking().Where(up => up.IsEnabled == isEnabled && up.IsDeleted == isDeleted).ToListAsync();
        }

        public async Task<PermissionResponse> GetAllUserPermisionFromUserAsync(string userId)
        {
            PermissionResponse response = new() { Success = false };

            try
            {
                response.UserPermissions = await _context.UserPermissions
                    .AsNoTracking()
                    .Include(p => p.Permission)
                    .Where(p => p.UserId == userId)
                    .ToListAsync();


                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
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

        public async Task<GeneralResponse> UpdatePermissionsAsync(List<UserPermission> userPermissions)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            using (var transacction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (UserPermission userPermission in userPermissions)
                    {
                        _context.Update(userPermission);
                    }


                    GeneralResponse result = await _dBHelper.SaveChangesAsync(_context);
                    if (result.Success)
                    {

                        transacction.Commit();
                        return result;
                    }
                    else
                    {
                        transacction.Rollback();
                        return result;
                    }

                }
                catch (Exception ex)
                {
                    transacction.Rollback();
                    return new GeneralResponse
                    {
                        Message = ex.Message,
                        Success = false,
                    };
                }
                finally
                {
                    StaticCommon.ContextInUse = false;
                }
            }
        }

        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permission && up.IsEnabled && !up.IsDeleted);
        }
    }
}
