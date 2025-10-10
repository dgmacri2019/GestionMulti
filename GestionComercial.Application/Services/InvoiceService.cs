using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace GestionComercial.Applications.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;
        public InvoiceService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }



        public async Task<InvoiceResponse> FindByIdAsync(int id)
        {
            try
            {
                return new InvoiceResponse
                {
                    Success = true,
                    Invoice = await _context.Invoices
                                    .AsNoTrackingWithIdentityResolution()
                                    .Include(id => id.InvoiceDetails)
                                    .Include(s => s.Sale)
                                    .Include(c => c.Client)
                                    .Include(ic => ic.IvaCondition)
                                    .Where(i => i.Id == id)
                                    .FirstOrDefaultAsync()
                };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<InvoiceResponse> FindBySaleIdAsync(int saleId, int compTypeId)
        {
            try
            {
                return new InvoiceResponse
                {
                    Success = true,
                    Invoice = await _context.Invoices
                                        .AsNoTrackingWithIdentityResolution()
                                        .Include(id => id.InvoiceDetails)
                                        .Include(s => s.Sale)
                                        .Include(c => c.Client)
                                        .Include(ic => ic.IvaCondition)
                                        .FirstOrDefaultAsync(i => i.SaleId == saleId && i.CompTypeId == compTypeId)
                };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<InvoiceResponse> FindBySaleIdAsync(int saleId)
        {
            try
            {
                return new InvoiceResponse
                {
                    Success = true,
                    Invoice = await _context.Invoices
                                        .AsNoTrackingWithIdentityResolution()
                                        .Include(id => id.InvoiceDetails)
                                        .Include(s => s.Sale)
                                        .Include(c => c.Client)
                                        .Include(ic => ic.IvaCondition)
                                        .FirstOrDefaultAsync(i => i.SaleId == saleId)
                };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<InvoiceResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {
                List<Invoice> invoices = await _context.Invoices
                                                .AsNoTrackingWithIdentityResolution()
                                                .Include(id => id.InvoiceDetails)
                                                .Include(s => s.Sale)
                                                .Include(c => c.Client)
                                                .Include(ic => ic.IvaCondition)
                                                .OrderBy(i => i.PtoVenta).ThenBy(i => i.CompNro)
                                                .Skip((page - 1) * pageSize)
                                                .Take(pageSize)
                                                .ToListAsync();

                return new InvoiceResponse
                {
                    Success = true,
                    Invoices = invoices,
                };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<InvoiceResponse> GetAllBySalePointAsync(int salePoint, DateTime saleDate, int page, int pageSize)
        {
            try
            {
                // 1️ Ejecuta en SQL todo lo posible (filtros, include, orden, paginación)
                var partialInvoices = await _context.Invoices
                    .AsNoTrackingWithIdentityResolution()
                    .Include(id => id.InvoiceDetails)
                    .Include(s => s.Sale)
                    .Include(c => c.Client)
                    .Include(ic => ic.IvaCondition)
                    .Where(i => i.PtoVenta == salePoint)
                    .OrderBy(i => i.PtoVenta)
                    .ThenBy(i => i.CompNro)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(); // ✅ hasta acá todo se ejecuta en SQL

                // 2 Filtra en memoria usando ParseExact
                List<Invoice> invoices = partialInvoices
                    .Where(i => DateTime.ParseExact(i.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture).Date == saleDate.Date)
                    .ToList();

                return new InvoiceResponse
                {
                    Success = true,
                    Invoices = invoices,
                };
            }
            catch (Exception ex)
            {
                return new InvoiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }
    }
}
