using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using Microsoft.AspNetCore.Identity;

namespace GestionComercial.Applications.Interfaces
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(string username, string password);
        Task<IdentityResult> AddAsync(UserDto model);
        Task<IdentityResult> DeleteAsync(string id);
        Task<IdentityResult> UpdateAsync(UserDto model);
        Task<IdentityResult> ChangeRoleAsync(UserDto model);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        
        
        
        


        IEnumerable<User> GetAll();
        User GetById(string id);
       
    }
}
