using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionComercial.Applications.Services
{
    public class SalesService : ISalesService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;


        public SalesService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }


        public async Task<IEnumerable<SaleViewModel>> GetAllAsync()
        {
            List<IGrouping<string, Sale>> sales = await _context.Sales
                .Include(c => c.Client)
                .Include(sc => sc.SaleCondition)
                .Include(sd => sd.SaleDetails)
                .Include(spm => spm.SalePayMetodDetails)
                .Include(a => a.Acreditations)
                //.Where(p => p.IsEnabled == isEnabled && p.IsDeleted == isDeleted)
                .OrderBy(sp => sp.SalePoint).ThenBy(sn => sn.SaleNumber)
                .GroupBy(c => c.Client.BusinessName)
                .ToListAsync();
        }

        public Task<SaleViewModel?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
