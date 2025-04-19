using Microsoft.AspNetCore.Identity;

namespace GestionComercial.Applications.Interfaces
{
    public interface IRoleService
    {
        Task<bool> AddAsync(string roleName);

        Task<bool> DeleteAsync(string roleId);

        Task<List<IdentityRole>> GetAllAsync();
        
        List<IdentityRole> GetAll();
    }
}
