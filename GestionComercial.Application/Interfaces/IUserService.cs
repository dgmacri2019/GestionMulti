using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(string id);
        Task<GeneralResponse> AddAsync(User user);
        Task<GeneralResponse> UpdateAsync(User user);
        Task<GeneralResponse> DeleteAsync(string id);


        IEnumerable<User> GetAll();
        User GetById(string id);
        GeneralResponse Add(User user);
        GeneralResponse Update(User user);
        GeneralResponse Delete(string id);
    }
}
