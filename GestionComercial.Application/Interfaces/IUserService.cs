using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Identity;

namespace GestionComercial.Applications.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<IdentityResult> AddAsync(UserFilterDto model);
        Task<IdentityResult> DeleteAsync(string id);
        Task<IdentityResult> UpdateAsync(UserFilterDto model);
        Task<IdentityResult> ChangeRoleAsync(UserFilterDto model);
        Task<IEnumerable<UserViewModel>> GetAllAsync(UserFilterDto model);
        Task<IEnumerable<UserViewModel>> SearchToListAsync(UserFilterDto model);
        Task<UserViewModel?> GetByIdAsync(UserFilterDto model);
    }
}
