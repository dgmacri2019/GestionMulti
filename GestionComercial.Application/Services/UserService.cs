using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
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
                    return new LoginResponse { Success = false, Token = null, Message = "Usuario o contraseña inválidos" };

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

        public async Task<IdentityResult> AddAsync(UserViewModel model)
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

            IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, model.RoleName);
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

        public async Task<IdentityResult> UpdateAsync(UserViewModel model)
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
            if (model.ChangePassword && string.IsNullOrEmpty(model.Password))
                user.ChangePassword = true;

            if (model.ChangePassword && !string.IsNullOrEmpty(model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);

                IdentityResult resultUpdate = await _userManager.UpdateAsync(user);
                if (!resultUpdate.Succeeded)
                    return resultUpdate;

                IdentityResult resultRemoveRole = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!resultRemoveRole.Succeeded)
                    return resultRemoveRole;
                IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!resultAddRole.Succeeded)
                    return resultAddRole;
                return await _userManager.AddPasswordAsync(user, model.Password);
            }
            else
                return await _userManager.UpdateAsync(user);



        }

        public async Task<IdentityResult> ChangeRoleAsync(UserViewModel model)
        {
            User? user = await _userManager.FindByIdAsync(model.Id);


            if (user == null)
                return new IdentityResult();

            IList<string> roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            return await _userManager.AddToRoleAsync(user, model.RoleName);
        }

        public async Task<UserResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {

                List<User> users = await _context.Users
                    .AsNoTracking()
                    .Include(up => up.UserPermissions)
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalRegisters = await _context.Users.AsNoTracking().CountAsync();

                return new UserResponse
                {
                    Success = true,
                    UserViewModels = await ToUserViewModelListAsync(users),
                    TotalRegisters = totalRegisters
                };
            }
            catch (Exception ex)
            {
                return new UserResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<UserViewModel?> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return new UserViewModel
                {
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    Email = string.Empty,
                    Id = string.Empty,
                    ChangePassword = true,
                    IsEnabled = true,
                    UserName = string.Empty,
                };

            User? user = await _context.Users.FindAsync(id);

            return user == null ? null : await ToUserViewModelAsync(user);
        }




        private async Task<List<UserViewModel>> ToUserViewModelListAsync(List<User> users)
        {
            List<UserRoleDto> userRoleDtos =
            [
                new UserRoleDto { Id = 0, Name = "Seleccione el Rol" },
                new UserRoleDto { Id = 1, Name = "Developer" },
                new UserRoleDto { Id = 2, Name = "Administrator" },
                new UserRoleDto { Id = 3, Name = "Supervisor" },
                new UserRoleDto { Id = 4, Name = "Operator" },
                new UserRoleDto { Id = 5, Name = "Cashier" }
            ];

            List<UserViewModel> result = [];

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault(); // string con el nombre del rol
                var roleId = userRoleDtos.FirstOrDefault(r => r.Name == roleName)?.Id ?? 0;

                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    RoleName = roleName,
                    ChangePassword = user.ChangePassword,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsDeleted = false,
                    IsEnabled = user.Enabled,
                    UserRoleDtos = userRoleDtos,
                    RoleId = roleId,

                });
            }

            return result;
        }

        public async Task<UserViewModel> ToUserViewModelAsync(User user)
        {
            List<UserRoleDto> userRoleDtos =
            [
                new UserRoleDto { Id = 0, Name = "Seleccione el Rol" },
                new UserRoleDto { Id = 1, Name = "Developer" },
                new UserRoleDto { Id = 2, Name = "Administrator" },
                new UserRoleDto { Id = 3, Name = "Supervisor" },
                new UserRoleDto { Id = 4, Name = "Operator" },
                new UserRoleDto { Id = 5, Name = "Cashier" }
            ];
            var roles = await _userManager.GetRolesAsync(user);
            string? roleName = roles.FirstOrDefault(); // string con el nombre del rol
            int roleId = userRoleDtos.FirstOrDefault(r => r.Name == roleName)?.Id ?? 0;

            return new UserViewModel
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                ChangePassword = user.ChangePassword,
                Email = user.Email,
                IsEnabled = user.Enabled,
                FullName = user.FullName,
                IsDeleted = false,
                PhoneNumber = user.PhoneNumber,
                UserRoleDtos = userRoleDtos,
                RoleId = roleId,
            };
        }


    }
}
