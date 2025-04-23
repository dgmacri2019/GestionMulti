using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GestionComercial.Applications.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;



        public UserService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }


        public GeneralResponse Add(User user)
        {           
            _context.Users.Add(user);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> AddAsync(User user)
        {
            _context.Users.Add(user);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse Delete(string id)
        {
            User user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                return _dBHelper.SaveChanges(_context);
            }
            return new GeneralResponse { Success = false, Message = "Usuario no encontrado" };
        }

        public async Task<GeneralResponse> DeleteAsync(string id)
        {
            User user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return new GeneralResponse { Success = false, Message = "Usuario no encontrado" };
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

        public GeneralResponse Update(User user)
        {
            _context.Users.Update(user);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            return await _dBHelper.SaveChangesAsync(_context);
        }
    }
}
