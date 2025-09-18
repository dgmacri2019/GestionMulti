using GestionComercial.Applications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;


        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


        public async Task<bool> AddAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                return result.Succeeded;
            }
            return false; // El rol ya existe
        }

        public async Task<bool> DeleteAsync(string roleId)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                return result.Succeeded;
            }
            return false;
        }

        public async Task<List<IdentityRole>> GetAllAsync()
        {
            return await _roleManager.Roles.AsNoTracking().ToListAsync();
        }

        public List<IdentityRole> GetAll()
        {
            return _roleManager.Roles.AsNoTracking().ToList();
        }
    }
}