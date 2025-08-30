using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

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
                .OrderBy(sp => sp.SalePoint).ThenBy(sn => sn.SaleNumber)
                .GroupBy(c => c.Client.BusinessName)
                .ToListAsync();

            ObservableCollection<Client> clients = new ObservableCollection<Client>(await _context.Clients
             .Include(c => c.PriceList)
             .Include(c => c.State)
             .Include(ic => ic.IvaCondition)
             .Include(dt => dt.DocumentType)
             .Include(sc => sc.SaleCondition)
             .Where(p => p.IsEnabled && !p.IsDeleted)
             .ToListAsync());

            ObservableCollection<SaleCondition> saleConditions = new ObservableCollection<SaleCondition>(await _context.SaleConditions
             .Where(sc => sc.IsEnabled && !sc.IsDeleted)
             .OrderBy(sc => sc.Description)
             .ToListAsync());

            ObservableCollection<PriceList> priceLists = new ObservableCollection<PriceList>(await _context.PriceLists
             .Where(pl => pl.IsEnabled && !pl.IsDeleted)
             .OrderBy(pl => pl.Description)
             .ToListAsync());


            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
            clients.Add(new Client { Id = 0, BusinessName = "Seleccione el cliente" });
            priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });

            return ToSaleViewModelsList(sales, clients, saleConditions, priceLists);


        }

        public async Task<SaleViewModel?> GetByIdAsync(int id)
        {
            ObservableCollection<Client> clients = new ObservableCollection<Client>(await _context.Clients
              .Include(c => c.PriceList)
              .Include(c => c.State)
              .Include(ic => ic.IvaCondition)
              .Include(dt => dt.DocumentType)
              .Include(sc => sc.SaleCondition)
              .Where(p => p.IsEnabled && !p.IsDeleted)
              .ToListAsync());

            ObservableCollection<SaleCondition> saleConditions = new ObservableCollection<SaleCondition>(await _context.SaleConditions
             .Where(sc => sc.IsEnabled && !sc.IsDeleted)
             .OrderBy(sc => sc.Description)
             .ToListAsync());

            ObservableCollection<PriceList> priceLists = new ObservableCollection<PriceList>(await _context.PriceLists
              .Where(pl => pl.IsEnabled && !pl.IsDeleted)
              .OrderBy(pl => pl.Description)
              .ToListAsync());


            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
            clients.Add(new Client { Id = 0, BusinessName = "Seleccione el cliente" });
            priceLists.Add(new PriceList { Id = 0, Description = "Seleccione la lista de precios" });


            Sale? sale = await _context.Sales
                .Include(c => c.Client)
                .Include(sc => sc.SaleCondition)
                .Include(sd => sd.SaleDetails)
                .Include(spm => spm.SalePayMetodDetails)
                .Include(a => a.Acreditations)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (id == 0)
                return new SaleViewModel
                {
                    Clients = clients,
                    SaleConditions = saleConditions,
                    PriceLists = priceLists,
                };

            return sale == null ? null : ConverterHelper.ToSaleViewModel(sale, clients, saleConditions, priceLists);
        }





        private IEnumerable<SaleViewModel> ToSaleViewModelsList(List<IGrouping<string, Sale>> sales,
           ObservableCollection<Client> clients, ObservableCollection<SaleCondition> saleConditions, ObservableCollection<PriceList> priceLists)
        {
            return sales.SelectMany(group => group.Select(sale => new SaleViewModel
            {
                Id = sale.Id,
                CreateDate = sale.CreateDate,
                CreateUser = sale.CreateUser,
                IsDeleted = sale.IsDeleted,
                IsEnabled = sale.IsEnabled,
                UpdateDate = sale.UpdateDate,
                UpdateUser = sale.UpdateUser,
                Acreditations = sale.Acreditations,
                AutorizationCode = sale.AutorizationCode,
                BaseImp105 = sale.BaseImp105,
                BaseImp21 = sale.BaseImp21,
                BaseImp27 = sale.BaseImp27,
                ClientId = sale.ClientId,
                GeneralDiscount = sale.GeneralDiscount,
                InternalTax = sale.InternalTax,
                IsFinished = sale.IsFinished,
                PaidOut = sale.PaidOut,
                PartialPay = sale.PartialPay,
                SaleConditionId = sale.SaleConditionId,
                SaleDate = sale.SaleDate,
                SaleDetails = sale.SaleDetails,
                SaleNumber = sale.SaleNumber,
                SalePayMetodDetails = sale.SalePayMetodDetails,
                SalePoint = sale.SalePoint,
                Sold = sale.Sold,
                SubTotal = sale.SubTotal,
                Total = sale.Total,
                TotalIVA105 = sale.TotalIVA105,
                TotalIVA21 = sale.TotalIVA21,
                TotalIVA27 = sale.TotalIVA27,
                Clients = clients,
                SaleConditions = saleConditions,
                PriceLists = priceLists,
            }));
        }
    }
}
