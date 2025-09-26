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
                .AsNoTracking()
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
                .AsNoTracking()
                 .Where(gp => gp.IsEnabled && !gp.IsDeleted && gp.PCName == pcName)
                 .FirstOrDefaultAsync();
            if (pcParameter == null)
            {
                pcParameter = new PcParameter
                {
                    PCName = pcName,
                    CreateDate = DateTime.Now,
                    CreateUser = "System",
                    LastLogin = DateTime.Now,
                    IsDeleted = false,
                    IsEnabled = true,
                    SalePoint = pcParameters == null || pcParameters.Count() == 0 ? 1 : pcParameters.Max(pc => pc.SalePoint) + 1,
                };
                await _context.AddAsync(pcParameter);
                await _dBHelper.SaveChangesAsync(_context);
            }
            else
            {
                PcParameter pcParameterUpdated = await _context.PcParameters.FindAsync(pcParameter.Id);
                pcParameterUpdated.LastLogin = DateTime.Now;
                pcParameter.LastLogin = DateTime.Now;
                _context.Update(pcParameterUpdated);
                await _dBHelper.SaveChangesAsync(_context);
            }
            return pcParameter;
        }

        public async Task<IEnumerable<PcSalePointsListViewModel>> GetAllPcParametersAsync()
        {
            ICollection<PcParameter> pcParameters = await _context.PcParameters
                .AsNoTracking()
                .Where(pp => pp.IsEnabled && !pp.IsDeleted)
                .ToListAsync();

            return ToPurchaseAndSalesList(pcParameters);
        }


        public async Task<IEnumerable<PcPrinterParametersListViewModel>> GetAllPcPrinterParametersAsync()
        {
            ICollection<PrinterParameter> printerParameters = await _context.PrinterParameters
                .AsNoTracking()
                .ToListAsync();

            return ToPcPrinterParametersList(printerParameters);
        }



        public async Task<IEnumerable<PrinterParameter>> GetPrinterParameterFromPcAsync(string pcName)
        {
            if (string.IsNullOrEmpty(pcName))
                return null;

            return await _context.PrinterParameters
                .AsNoTracking()
                .Where(ppp => ppp.ComputerName == pcName)
                .ToListAsync();
        }



        public async Task<PcParameter?> GetPcParameterByIdAsync(int id)
        {
            return await _context.PcParameters.FindAsync(id);
        }




        private IEnumerable<PcSalePointsListViewModel> ToPurchaseAndSalesList(ICollection<PcParameter> pcParameters)
        {
            return pcParameters.Select(pcParameter => new PcSalePointsListViewModel
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
                LastLogin = pcParameter.LastLogin,
            });
        }



        private IEnumerable<PcPrinterParametersListViewModel> ToPcPrinterParametersList(ICollection<PrinterParameter> printerParameters)
        {
            return printerParameters.Select(pcPrinterParameter => new PcPrinterParametersListViewModel
            {
                BarCodePrinter = pcPrinterParameter.BarCodePrinter,
                BudgetPrinter = pcPrinterParameter.BudgetPrinter,
                ComputerName = pcPrinterParameter.ComputerName,
                RemitPrinter = pcPrinterParameter.RemitPrinter,
                CreateDate = pcPrinterParameter.CreateDate,
                CreateUser = pcPrinterParameter.CreateUser,
                EnablePrintBarCode = pcPrinterParameter.EnablePrintBarCode,
                EnablePrintBudget = pcPrinterParameter.EnablePrintBudget,
                EnablePrintInvoice = pcPrinterParameter.EnablePrintInvoice,
                EnablePrintOrder = pcPrinterParameter.EnablePrintOrder,
                EnablePrintRemit = pcPrinterParameter.EnablePrintRemit,
                EnablePrintSale = pcPrinterParameter.EnablePrintSale,
                EnablePrintTicketChange = pcPrinterParameter.EnablePrintTicketChange,
                Id = pcPrinterParameter.Id,
                InvoicePrinter = pcPrinterParameter.InvoicePrinter,
                IsDeleted = pcPrinterParameter.IsDeleted,
                IsEnabled = pcPrinterParameter.IsEnabled,
                MaxWidthBarCodePrinter = pcPrinterParameter.MaxWidthBarCodePrinter,
                MaxWidthBudgetPrinter = pcPrinterParameter.MaxWidthBudgetPrinter,
                MaxWidthInvoicePrinter = pcPrinterParameter.MaxWidthInvoicePrinter,
                MaxWidthOrderPrinter = pcPrinterParameter.MaxWidthOrderPrinter,
                MaxWidthRemitPrinter = pcPrinterParameter.MaxWidthRemitPrinter,
                MaxWidthSalePrinter = pcPrinterParameter.MaxWidthSalePrinter,
                MaxWidthTicketChangePrinter = pcPrinterParameter.MaxWidthTicketChangePrinter,
                OrderPrinter = pcPrinterParameter.OrderPrinter,
                SalePoint = pcPrinterParameter.SalePoint,
                SalePrinter = pcPrinterParameter.SalePrinter,
                TicketChangePrinter = pcPrinterParameter.TicketChangePrinter,
                UpdateDate = pcPrinterParameter.UpdateDate,
                UpdateUser = pcPrinterParameter.UpdateUser,
                UseAllPrinters = pcPrinterParameter.UseAllPrinters,
                UseContinuousBarCodePrinter = pcPrinterParameter.UseContinuousBarCodePrinter,
                UseContinuousBudgetPrinter = pcPrinterParameter.UseContinuousBudgetPrinter,
                UseContinuousInvoicePrinter = pcPrinterParameter.UseContinuousInvoicePrinter,
                UseContinuousOrderPrinter = pcPrinterParameter.UseContinuousOrderPrinter,
                UseContinuousRemitPrinter = pcPrinterParameter.UseContinuousRemitPrinter,
                UseContinuousSalePrinter = pcPrinterParameter.UseContinuousSalePrinter,
                UseContinuousTicketChangePrinter = pcPrinterParameter.UseContinuousTicketChangePrinter,                 
            });
        }
    }
}
