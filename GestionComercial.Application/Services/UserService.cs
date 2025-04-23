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
        private readonly DBHelper _dBHelper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _dBHelper = new DBHelper();
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

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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

        public async Task<IdentityResult> AddAsync(UserDto model)
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

        public async Task<IdentityResult> UpdateAsync(UserDto model)
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

        public async Task<IdentityResult> ChangeRoleAsync(UserDto model)
        {
            User? user = await _userManager.FindByIdAsync(model.Id);


            if (user == null)
                return new IdentityResult();

            IList<string> roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);

            return await _userManager.AddToRoleAsync(user, model.Role);
        }


        public IEnumerable<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public User GetById(string id)
        {
            return _context.Users.Find(id);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }



    }
}
