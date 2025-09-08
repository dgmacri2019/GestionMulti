using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
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

            List<PcParameter> pcParameters = await _context.PcParameters.ToListAsync();

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
                    SalePoint = pcParameters == null || pcParameters.Count() == 0 ? 1 : pcParameters.Max(pc => pc.SalePoint) + 1,
                };
                await _context.AddAsync(pcParameter);
                await _dBHelper.SaveChangesAsync(_context);
            }
            return pcParameter;

        }

        public async Task<IEnumerable<PurchaseAndSalesListViewModel>> GetAllPcParametersAsync()
        {
            ICollection<PcParameter> pcParameters = await _context.PcParameters
                .Where(pp => pp.IsEnabled && !pp.IsDeleted)
                .ToListAsync();

            return ToPurchaseAndSalesList(pcParameters);
        }

        private IEnumerable<PurchaseAndSalesListViewModel> ToPurchaseAndSalesList(ICollection<PcParameter> pcParameters)
        {
            return pcParameters.Select(pcParameter => new PurchaseAndSalesListViewModel
            {
                Id = pcParameter.Id,
                CreateDate = pcParameter.CreateDate,
                CreateUser = pcParameter.CreateUser,
                IsDeleted = pcParameter.IsDeleted,
                IsEnabled = pcParameter.IsEnabled,
                PcName = pcParameter.PCName,
                SalePoint = pcParameter.SalePoint,
                UpdateDate = pcParameter.UpdateDate,
                UpdateUser = pcParameter.UpdateUser,
            });
        }

        public async Task<PcParameter?> GetPcParameterByIdAsync(int id)
        {
            return await _context.PcParameters.FindAsync(id);
        }
    }
}
