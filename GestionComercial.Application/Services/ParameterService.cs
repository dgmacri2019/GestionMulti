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




        public async Task<PcParameter?> GetPcParameterAsync(string pcName)
        {
            if (string.IsNullOrEmpty(pcName))
                return null;

            PcParameter? pcParameter = await _context.PcParameters
                 .Where(gp => gp.IsEnabled && !gp.IsDeleted && gp.PCName == pcName)
                 .FirstOrDefaultAsync();
            if (pcParameter == null)
            {
                pcParameter = new PcParameter
                {
                    PCName = pcName,
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    IsDeleted = false,
                    IsEnabled = true,
                    SalePoint = 0,
                };
                await _context.AddAsync(pcParameter);
                await _dBHelper.SaveChangesAsync(_context);
            }
            return pcParameter;

        }

        public async Task<PcParameter?> GetPcParameterByIdAsync(int id)
        {
            return await _context.PcParameters.FindAsync(id);
        }
    }
}
