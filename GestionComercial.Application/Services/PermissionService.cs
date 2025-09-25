using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Security;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static GestionComercial.Domain.Constant.Enumeration;

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
            return await _context.Permissions.AsNoTracking().Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted).ToListAsync();
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
            PermissionResponse response = new PermissionResponse { Success = false };

            try
            {
                List<UserPermission> userPermissions = await _context.UserPermissions.ToListAsync();

                foreach (ModuleType module in Enum.GetValues(typeof(ModuleType)))
                {
                    PermissionViewModel vm = new() { Module = module };
                    
                    // Buscar permisos disponibles en este módulo
                    List<Permission> modulePermissions = await _context.Permissions.Where(p => p.ModuleType == module).ToListAsync();

                    vm.CanRead = modulePermissions.Any(p => p.Name.EndsWith("Lectura") && userPermissions.Any(up => up.PermissionId == p.Id && up.UserId == userId && up.IsEnabled));
                    vm.CanAdd = modulePermissions.Any(p => p.Name.EndsWith("Agregar") && userPermissions.Any(up => up.PermissionId == p.Id && up.UserId == userId && up.IsEnabled));
                    vm.CanEdit = modulePermissions.Any(p => p.Name.EndsWith("Editar") && userPermissions.Any(up => up.PermissionId == p.Id && up.UserId == userId && up.IsEnabled));
                    vm.CanDelete = modulePermissions.Any(p => p.Name.EndsWith("Borrar") && userPermissions.Any(up => up.PermissionId == p.Id && up.UserId == userId && up.IsEnabled));

                    response.PermissionViewModels.Add(vm);
                }
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

        public async Task<bool> UserHasPermissionAsync(string userId, string permission)
        {
            return await _context.UserPermissions
                .AnyAsync(up => up.UserId == userId && up.Permission.Name == permission && up.IsEnabled && !up.IsDeleted);
        }
    }
}
