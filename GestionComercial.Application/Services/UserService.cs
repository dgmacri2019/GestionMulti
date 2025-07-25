using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionComercial.Applications.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        //private readonly DBHelper _dBHelper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            //_dBHelper = new DBHelper();
            _configuration = configuration;
        }




        public async Task<LoginResponse> LoginAsync(string username, string password)
        {
            try
            {
                User? user = await _userManager.FindByNameAsync(username);
                if (user == null || !(await _userManager.CheckPasswordAsync(user, password)))
                    return new LoginResponse { Success = false, Token = null };

                IList<string> roles = await _userManager.GetRolesAsync(user);

                List<Claim> claims =
                [
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    .. roles.Select(role => new Claim(ClaimTypes.Role, role)),
                ];

                byte[] key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                JwtSecurityToken token = new(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(12),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
                );

                return new LoginResponse
                {
                    Success = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<IdentityResult> AddAsync(UserFilterDto model)
        {
            User user = new()
            {
                UserName = model.UserName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Enabled = model.IsEnabled,
                PhoneNumber = model.PhoneNumber,
            };
            IdentityResult resultAddUser = await _userManager.CreateAsync(user, model.Password);
            if (!resultAddUser.Succeeded)
                return resultAddUser;

            IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, model.Role);
            if (!resultAddRole.Succeeded)
                await _userManager.DeleteAsync(user);

            return resultAddRole;
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return new IdentityResult();
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateAsync(UserFilterDto model)
        {
            User? user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return new IdentityResult();

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Enabled = model.IsEnabled;

            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> ChangeRoleAsync(UserFilterDto model)
        {
            User? user = await _userManager.FindByIdAsync(model.Id);


            if (user == null)
                return new IdentityResult();

            IList<string> roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            return await _userManager.AddToRoleAsync(user, model.Role);
        }

        public async Task<IEnumerable<UserViewModel>> GetAllAsync(UserFilterDto model)
        {

            List<User> users = model.All ?
                await _context.Users.ToListAsync()
                :
                await _context.Users.Where(u => u.Enabled == model.IsEnabled).ToListAsync();

            return ToUserViewModelList(users);
        }

        public async Task<IEnumerable<UserViewModel>> SearchToListAsync(UserFilterDto model)
        {
            List<User> users = model.All ?
                await _context.Users.ToListAsync()
                :
                string.IsNullOrEmpty(model.NameFilter) ?
                    await _context.Users
                    .Where(u => u.Enabled == model.IsEnabled)
                    .ToListAsync()
                    :
                    await _context.Users
                    .Where(u => u.Enabled == model.IsEnabled && (u.UserName.Contains(model.NameFilter)
                                                            || u.FirstName.Contains(model.NameFilter)
                                                            || u.LastName.Contains(model.NameFilter)
                                                            || u.Email.Contains(model.NameFilter)))
                    .ToListAsync();

            return ToUserViewModelList(users);
        }

        public async Task<UserViewModel?> GetByIdAsync(UserFilterDto model)
        {
            if (string.IsNullOrEmpty(model.Id))
                return new UserViewModel
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty,
                    Id = string.Empty,
                    ChangePassword = true,
                    Enabled = true,
                    UserName = string.Empty,
                };

            User? user = await _context.Users.FindAsync(model.Id);

            return user == null ? null : ConverterHelper.ToUserViewModel(user);
        }




        private IEnumerable<UserViewModel> ToUserViewModelList(List<User> users)
        {
            return users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Enabled = user.Enabled,
                FullName = user.FullName,
                UserName = user.UserName,
                ChangePassword = user.ChangePassword,
                Email = user.Email,
                Phone = user.PhoneNumber,
            });
        }

    }
}
