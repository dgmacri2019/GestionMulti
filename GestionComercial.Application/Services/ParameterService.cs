using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Applications.Services
{
    public class ParameterService : IParameterService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        public ParameterService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }

        public async Task<IEnumerable<GeneralParameter>> GetAllGeneralParametersAsync()
        {
            return await _context.GeneralParameters
                .Where(gp => gp.IsEnabled && !gp.IsDeleted)
                .ToListAsync();
        }

        public async Task<GeneralParameter?> GetGeneralParameterByIdAsync(int id)
        {
            return await _context.GeneralParameters.FindAsync(id);
        }
    }
}
