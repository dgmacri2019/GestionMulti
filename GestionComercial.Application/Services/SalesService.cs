using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

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

        public async Task<SaleResponse> AddAsync(Sale sale)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            using (var transacction = _context.Database.BeginTransaction())
            {
                try
                {
                    StaticCommon.ContextInUse = true;
                    Client? client = await _context.Clients.FindAsync(sale.ClientId);
                    if (client == null)
                    {
                        transacction.Rollback();
                        return new SaleResponse
                        {
                            Message = "No se reconoce el cliente.",
                            Success = false,
                        };
                    }
                    if (sale.SaleDetails.Count <= 0)
                    {
                        transacction.Rollback();
                        return new SaleResponse
                        {
                            Message = "No hay artículos cargados.",
                            Success = false,
                        };

                    }
                    foreach (SaleDetail saleDetail in sale.SaleDetails)
                    {
                        Article? article = await _context.Articles.FindAsync(saleDetail.ArticleId);
                        if (article == null)
                        {
                            transacction.Rollback();
                            return new SaleResponse
                            {
                                Message = "No se reconoce el artículo.",
                                Success = false,
                            };
                        }
                        if (article != null && article.StockCheck)
                        {
                            article.Stock -= saleDetail.Quantity;
                            _context.Update(article);
                        }
                    }
                    client.Sold += sale.Total - sale.SalePayMetodDetails.Sum(sp => sp.Value);
                    _context.Update(client);

                    await _context.AddAsync(sale);
                    StaticCommon.ContextInUse = false;
                    GeneralResponse resultSave = await _dBHelper.SaveChangesAsync(_context);
                    if (resultSave.Success)
                    {
                        StaticCommon.ContextInUse = true;
                        transacction.Commit();
                        StaticCommon.ContextInUse = false;
                        return new SaleResponse { Success = true, SaleId = sale.Id };
                    }
                    else
                        return new SaleResponse { Success = false, Message = resultSave.Message };
                }
                catch (Exception ex)
                {
                    transacction.Rollback();
                    return new SaleResponse
                    {
                        Message = ex.Message,
                        Success = false,
                    };
                }
                finally
                {
                    StaticCommon.ContextInUse = false;

                }
            }
        }

        public async Task<SaleResponse> GetAllAsync(int page, int pageSize)
        {
            try
            {
                List<Sale> sales = await _context.Sales
                       .AsNoTracking()
                       .Include(c => c.Client)
                       //.Include(sc => sc.SaleCondition)
                       .Include(sd => sd.SaleDetails)
                       .Include(spm => spm.SalePayMetodDetails)
                       .Include(a => a.Acreditations)
                       .OrderBy(sp => sp.SalePoint).ThenBy(sn => sn.SaleNumber)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();


                return new SaleResponse
                {
                    Success = true,
                    SaleViewModels = ToSaleViewModelsList(sales),
                };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }

        }

        public async Task<SaleResponse> GetAllBySalePointAsync(int salePoint, DateTime saleDate, int page, int pageSize)
        {
            try
            {
                List<Sale> sales = await _context.Sales
                       .AsNoTracking()
                       .Include(c => c.Client)
                       //.Include(sc => sc.SaleCondition)
                       .Include(sd => sd.SaleDetails)
                       .Include(spm => spm.SalePayMetodDetails)
                       .Include(a => a.Acreditations)
                       .Where(s => s.SalePoint == salePoint && s.SaleDate == saleDate.Date)
                       .OrderBy(sp => sp.SalePoint).ThenBy(sn => sn.SaleNumber)
                       .Skip((page - 1) * pageSize)
                       .Take(pageSize)
                       .ToListAsync();


                return new SaleResponse
                {
                    Success = true,
                    SaleViewModels = ToSaleViewModelsList(sales),
                };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }

        }

        public async Task<SaleResponse> GetByIdAsync(int id)
        {
            try
            {
                Sale? sale = await _context.Sales
                        .AsNoTracking()
                        .Include(c => c.Client)
                        //.Include(sc => sc.SaleCondition)
                        .Include(sd => sd.SaleDetails)
                        .Include(spm => spm.SalePayMetodDetails)
                        .Include(a => a.Acreditations)
                        .Where(s => s.Id == id)
                        .FirstOrDefaultAsync();

                return new SaleResponse
                {
                    Success = true,
                    SaleViewModel = id == 0 || sale == null ? new SaleViewModel() : ConverterHelper.ToSaleViewModel(sale),
                };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<SaleResponse> GetLastSaleNumber(int salePoint)
        {
            try
            {
                int? lastSaleNumber = await _context.Sales
                    .AsNoTracking()
                    .Where(s => s.SalePoint == salePoint)
                    .Select(s => (int?)s.SaleNumber) // usamos nullable para manejar el caso de que no haya registros
                    .MaxAsync();

                return new SaleResponse
                {
                    Success = true,
                    LastSaleNumber = lastSaleNumber == null ? 0 : lastSaleNumber.Value
                };
            }
            catch (Exception ex)
            {
                return new SaleResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }


        #region Invoice

        public async Task<Invoice?> FindInvoiceBySaleIdAsync(int saleId, int compTypeId)
        {
            return await _context.Invoices
                .AsNoTracking()
                .Include(iv => iv.InvoiceDetails)
                .FirstOrDefaultAsync(i => i.SaleId == saleId && i.CompTypeId == compTypeId);
        }


        #endregion


        private List<SaleViewModel> ToSaleViewModelsList(List<Sale> sales)
        {
            return sales.Select(sale => new SaleViewModel
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
                Client = sale.Client,
                Date = sale.SaleDate,
                //Clients = clients,
                //SaleConditions = saleConditions,
                //PriceLists = priceLists,
                //SaleCondition = sale.SaleCondition,
                //SaleConditionId = sale.SaleConditionId,


            }).OrderBy(s => s.SalePoint).ThenBy(s => s.SaleNumber).ToList();
        }
    }
}
