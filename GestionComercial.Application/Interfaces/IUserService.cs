using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Identity;

namespace GestionComercial.Applications.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<UserResponse> AddAsync(UserViewModel model);
        Task<IdentityResult> DeleteAsync(string id);
        Task<UserResponse> UpdateAsync(UserViewModel model);
        Task<IdentityResult> ChangeRoleAsync(UserViewModel model);
        Task<UserResponse> GetAllAsync(int page, int pageSize);
        Task<UserViewModel?> GetByIdAsync(string id);
    }
}
