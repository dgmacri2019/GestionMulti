using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Security;
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
        private List<UserRoleDto> UserRoleDtos =
            [
            new UserRoleDto { Id = 0, Name = "Seleccione el Rol" },
            new UserRoleDto { Id = 1, Name = "Developer"},
            new UserRoleDto { Id = 2, Name = "Administrador" },
            new UserRoleDto { Id = 3, Name = "Supervisor" },
            new UserRoleDto { Id = 4, Name = "Operador"},
            new UserRoleDto { Id = 5, Name = "Cajero" }
            ];
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
                if (user == null)
                {
                    return new LoginResponse { Success = false, Token = null, Message = "Usuario o contraseña inválidos" };
                }

                if (!user.Enabled)
                    return new LoginResponse { Success = false, Token = null, Message = "Usuario Inhabilitado" };
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
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    UserId = user.Id,
                    Permissions = await _context.Permissions
                                                    .Include(up => up.UserPermissions)
                                                    .Include(rp => rp.RolePermissions)
                                                    .Where(up => up.UserPermissions.Count(x => x.UserId == user.Id && x.IsEnabled) > 0)
                                                    .ToListAsync(),
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

        public async Task<UserResponse> AddAsync(UserViewModel model)
        {
            UserResponse userResponse = new() { Success = false };

            try
            {
                User user = new()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Enabled = model.IsEnabled,
                    PhoneNumber = model.PhoneNumber,
                    EmailConfirmed = true,
                };
                IdentityResult resultAddUser = await _userManager.CreateAsync(user, model.Password);
                if (!resultAddUser.Succeeded)
                    if (!resultAddUser.Succeeded)
                    {
                        userResponse.Message = $"Error: codigo {resultAddUser.Errors.First().Code}.\n{resultAddUser.Errors.First().Description}";
                        return userResponse;
                    }

                IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!resultAddRole.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    userResponse.Message = $"Error: codigo {resultAddRole.Errors.First().Code}.\n{resultAddRole.Errors.First().Description}";
                    return userResponse;
                }
                List<RolePermission> rolePermissions = await _context.RolePermissions
                                    .Where(rp => rp.RoleId == model.RoleId.ToString())
                                    .ToListAsync();

                foreach (RolePermission rolePermission in rolePermissions)
                {
                    UserPermission? userPermission = await _context.UserPermissions
                        .Where(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId)
                        .FirstOrDefaultAsync();
                    if (userPermission == null)
                        await _context.UserPermissions.AddAsync(new UserPermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = model.CreateUser,
                            IsDeleted = false,
                            IsEnabled = rolePermission.IsEnabled,
                            UserId = user.Id,
                            PermissionId = rolePermission.PermissionId,
                        });
                    else
                    {
                        userPermission.UpdateDate = DateTime.Now;
                        userPermission.UpdateUser = model.CreateUser;
                        userPermission.IsEnabled = rolePermission.IsEnabled;
                        _context.Update(userPermission);
                    }
                }
                await _context.SaveChangesAsync();

                userResponse.Success = true;
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.Message = ex.Message;
                return userResponse;
            }
        }

        public async Task<IdentityResult> DeleteAsync(string id)
        {
            User? user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return new IdentityResult();
            return await _userManager.DeleteAsync(user);
        }

        public async Task<UserResponse> UpdateAsync(UserViewModel model)
        {
            UserResponse userResponse = new UserResponse { Success = false, };
            try
            {
                User? user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    userResponse.Message = "No se reconoce el usuario.";
                    return userResponse;
                }

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
                    IdentityResult resultRemovePass = await _userManager.RemovePasswordAsync(user);
                    if (!resultRemovePass.Succeeded)
                    {
                        userResponse.Message = $"Error: codigo {resultRemovePass.Errors.First().Code}.\n{resultRemovePass.Errors.First().Description}";
                        return userResponse;
                    }
                    IdentityResult resultAddPassword = await _userManager.AddPasswordAsync(user, model.Password);
                    if (!resultAddPassword.Succeeded)
                    {
                        userResponse.Message = $"Error: codigo {resultAddPassword.Errors.First().Code}.\n{resultAddPassword.Errors.First().Description}";
                        return userResponse;
                    }
                }
                // IdentityResult resultAddRole1 = await _userManager.AddToRoleAsync(user, model.RoleName);
                var roles = await _userManager.GetRolesAsync(user);

                IdentityResult resultRemoveRole = await _userManager.RemoveFromRolesAsync(user, roles);
                if (!resultRemoveRole.Succeeded)
                    if (!resultRemoveRole.Succeeded)
                    {
                        userResponse.Message = $"Error: codigo {resultRemoveRole.Errors.First().Code}.\n{resultRemoveRole.Errors.First().Description}";
                        return userResponse;
                    }

                foreach (UserPermission userPermission in await _context.UserPermissions.Where(up => up.UserId == user.Id).ToListAsync())
                {
                    if (userPermission != null)
                    {
                        userPermission.UpdateDate = DateTime.Now;
                        userPermission.UpdateUser = model.CreateUser;
                        userPermission.IsEnabled = false;
                        _context.Update(userPermission);
                    }
                }
                await _context.SaveChangesAsync();


                IdentityResult resultAddRole = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!resultAddRole.Succeeded)
                {
                    userResponse.Message = $"Error: codigo {resultAddRole.Errors.First().Code}.\n{resultAddRole.Errors.First().Description}";
                    return userResponse;
                }

                List<RolePermission> rolePermissions = await _context.RolePermissions
                               .Where(rp => rp.RoleId == model.RoleId.ToString())
                               .ToListAsync();

                foreach (RolePermission rolePermission in rolePermissions)
                {
                    UserPermission? userPermission = await _context.UserPermissions
                        .Where(up => up.UserId == user.Id && up.PermissionId == rolePermission.PermissionId)
                        .FirstOrDefaultAsync();
                    if (userPermission == null)
                        await _context.UserPermissions.AddAsync(new UserPermission
                        {
                            CreateDate = DateTime.Now,
                            CreateUser = model.CreateUser,
                            IsDeleted = false,
                            IsEnabled = rolePermission.IsEnabled,
                            UserId = user.Id,
                            PermissionId = rolePermission.PermissionId,
                        });
                    else
                    {
                        userPermission.UpdateDate = DateTime.Now;
                        userPermission.UpdateUser = model.CreateUser;
                        userPermission.IsEnabled = rolePermission.IsEnabled;
                        _context.Update(userPermission);
                    }
                }
                await _context.SaveChangesAsync();

                IdentityResult resultUpdate = await _userManager.UpdateAsync(user);
                if (!resultUpdate.Succeeded)
                {
                    userResponse.Message = $"Error: codigo {resultUpdate.Errors.First().Code}.\n{resultUpdate.Errors.First().Description}";
                    return userResponse;
                }
                userResponse.Success = true;
                return userResponse;
            }
            catch (Exception ex)
            {
                userResponse.Message = ex.Message;
                return userResponse;
            }

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
                    .AsNoTrackingWithIdentityResolution()
                    .Include(up => up.UserPermissions)
                    .OrderBy(u => u.FirstName)
                    .ThenBy(u => u.LastName)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var totalRegisters = await _context.Users.AsNoTrackingWithIdentityResolution().CountAsync();

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
            List<UserViewModel> result = [];

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault(); // string con el nombre del rol
                var roleId = UserRoleDtos.FirstOrDefault(r => r.Name == roleName)?.Id ?? 0;

                result.Add(new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    FullName = user.FullName,
                    UserName = user.UserName,
                    RoleName = UserRoleDtos.First(urd => urd.Id == roleId).Name,
                    ChangePassword = user.ChangePassword,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    IsDeleted = false,
                    IsEnabled = user.Enabled,
                    UserRoleDtos = UserRoleDtos,
                    RoleId = roleId,

                });
            }

            return result;
        }

        private async Task<UserViewModel> ToUserViewModelAsync(User user)
        {

            var roles = await _userManager.GetRolesAsync(user);
            string? roleName = roles.FirstOrDefault(); // string con el nombre del rol
            int roleId = UserRoleDtos.FirstOrDefault(r => r.Name == roleName)?.Id ?? 0;

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
                UserRoleDtos = UserRoleDtos,
                RoleId = roleId,
            };
        }


    }
}
