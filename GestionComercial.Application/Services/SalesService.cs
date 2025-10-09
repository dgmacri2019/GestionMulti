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
using System.Globalization;

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
                    {
                        transacction.Rollback();
                        return new SaleResponse { Success = false, Message = resultSave.Message };
                    }
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

        public async Task<SaleResponse> AnullAsync(Sale sale, string userName)
        {
            while (StaticCommon.ContextInUse)
                await Task.Delay(100);

            using (var transacction = _context.Database.BeginTransaction())
            {
                try
                {
                    StaticCommon.ContextInUse = true;

                    if (sale != null)
                    {
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

                        Sale newSale = new()
                        {
                            AutorizationCode = sale.AutorizationCode,
                            BaseImp105 = sale.BaseImp105 * -1,
                            BaseImp21 = sale.BaseImp21 * -1,
                            BaseImp27 = sale.BaseImp27 * -1,
                            Client = sale.Client,
                            ClientId = sale.ClientId,
                            CreateDate = DateTime.Now,
                            CreateUser = userName,
                            GeneralDiscount = sale.GeneralDiscount * -1,
                            InternalTax = sale.InternalTax * -1,
                            IsDeleted = sale.IsDeleted,
                            IsEnabled = sale.IsEnabled,
                            IsFinished = sale.IsFinished,
                            PaidOut = sale.PaidOut,
                            SaleDate = sale.SaleDate,
                            SaleNumber = sale.SaleNumber,
                            SalePoint = sale.SalePoint,
                            SubTotal = sale.SubTotal * -1,
                            Total = sale.Total * -1,
                            TotalIVA105 = sale.TotalIVA105 * -1,
                            TotalIVA21 = sale.TotalIVA21 * -1,
                            TotalIVA27 = sale.TotalIVA27 * -1,
                            BaseImp0 = sale.BaseImp0 * -1,
                            BaseImp25 = sale.BaseImp25 * -1,
                            BaseImp5 = sale.BaseImp5 * -1,
                            PartialPay = sale.PartialPay,
                            Sold = sale.Sold,
                            TotalIVA25 = sale.TotalIVA25 * -1,
                            TotalIVA5 = sale.TotalIVA5 * -1,



                            //SaleCondition = sale.SaleCondition,

                        };
                        await _context.AddAsync(newSale);
                        GeneralResponse resultAdd = await _dBHelper.SaveChangesAsync(_context);
                        if (resultAdd.Success)
                        {
                            List<SaleDetail> saleDetails = [];
                            foreach (SaleDetail saleDetail in sale.SaleDetails)
                            {
                                Article article = await _context.Articles.FindAsync(saleDetail.ArticleId);
                                await _context.AddAsync(new SaleDetail
                                {
                                    Code = saleDetail.Code,
                                    CreateDate = DateTime.Now,
                                    CreateUser = userName,
                                    Description = saleDetail.Description,
                                    Discount = saleDetail.Discount,
                                    IsDeleted = saleDetail.IsDeleted,
                                    IsEnabled = saleDetail.IsEnabled,
                                    List = saleDetail.List,
                                    Price = saleDetail.Price,
                                    PriceDiscount = saleDetail.PriceDiscount,
                                    ArticleId = saleDetail.ArticleId,
                                    Quantity = saleDetail.Quantity,
                                    Sale = newSale,
                                    SaleId = newSale.Id,
                                    SubTotal = saleDetail.SubTotal,
                                    Tax = saleDetail.Tax,
                                    TaxId = saleDetail.TaxId,
                                    TotalItem = saleDetail.TotalItem,
                                    Article = article,
                                });

                                if (article.StockCheck)
                                {
                                    article.Stock += saleDetail.Quantity;
                                    _context.Update(article);
                                }
                            }

                            /*
                            foreach (SalePayMetodDetail salePayMetod in sale.SalePayMetodDetails)
                            {
                                salePayMetod.IsDeleted = true;
                                salePayMetod.IsEnabled = false;
                                salePayMetod.UpdateDate = DateTime.Now;
                                salePayMetod.UpdateUser = UserCache.UserName;
                                await _generalGestor.UpdateAsync(salePayMetod);
                                switch (salePayMetod.SaleCondition)
                                {
                                    case SaleCondition.EfectivoPeso:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == salePayMetod.SaleCondition).FirstOrDefault();
                                            box.Sold -= salePayMetod.Value;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoDolar:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == salePayMetod.SaleCondition).FirstOrDefault();
                                            box.Sold -= salePayMetod.Value;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoReal:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == salePayMetod.SaleCondition).FirstOrDefault();
                                            box.Sold -= salePayMetod.Value;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoEuro:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == salePayMetod.SaleCondition).FirstOrDefault();
                                            box.Sold -= salePayMetod.Value;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoOtro:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == salePayMetod.SaleCondition).FirstOrDefault();
                                            box.Sold -= salePayMetod.Value;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.CtaCte:
                                        {
                                            break;
                                        }
                                    case SaleCondition.Cheque:
                                        {
                                            break;
                                        }
                                    case SaleCondition.MercadoPagoTransf:
                                        {
                                            BankParameter bankParameter = StaticBoxAndBank.BankParameters.Where(bp => bp.SaleCondition == salePayMetod.SaleCondition).First();
                                            Bank bank = StaticBoxAndBank.FindBank(bankParameter.BankId);
                                            decimal fromAcredit = salePayMetod.Value - (salePayMetod.Value * bankParameter.Rate / 100);
                                            bank.Sold -= fromAcredit;
                                            bank.UpdateDate = DateTime.Now;
                                            bank.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(bank);
                                            break;
                                        }
                                    case SaleCondition.Deposito:
                                        {
                                            BankParameter bankParameter = StaticBoxAndBank.BankParameters.Where(bp => bp.SaleCondition == salePayMetod.SaleCondition).First();
                                            Bank bank = StaticBoxAndBank.FindBank(bankParameter.BankId);
                                            decimal fromAcredit = salePayMetod.Value - (salePayMetod.Value * bankParameter.Rate / 100);
                                            bank.Sold -= fromAcredit;
                                            bank.UpdateDate = DateTime.Now;
                                            bank.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(bank);
                                            break;
                                        }
                                    default:
                                        {
                                            BankParameter bankParameter = StaticBoxAndBank.BankParameters.Where(bp => bp.SaleCondition == salePayMetod.SaleCondition).First();
                                            Bank bank = StaticBoxAndBank.FindBank(bankParameter.BankId);

                                            Acreditation acreditation = StaticAcreditation.FindFromSaleId(sale.Id);
                                            if (acreditation != null)
                                            {
                                                acreditation.IsEnabled = false;
                                                acreditation.IsDeleted = false;
                                                acreditation.UpdateDate = DateTime.Now;
                                                acreditation.UpdateUser = UserCache.UserName;

                                                await _generalGestor.UpdateAsync(acreditation);
                                                if (acreditation.IsAcredited)
                                                    bank.Sold -= acreditation.FromAcredit;
                                                else
                                                    bank.FromCredit -= acreditation.FromAcredit;
                                                bank.UpdateDate = DateTime.Now;
                                                bank.UpdateUser = UserCache.UserName;
                                                await _generalGestor.UpdateAsync(bank);
                                            }
                                            break;
                                        }
                                }
                            }
                            if (sale.SalePayMetodDetails == null)
                                switch (sale.SaleCondition)
                                {
                                    case SaleCondition.EfectivoPeso:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == sale.SaleCondition).FirstOrDefault();
                                            box.Sold -= sale.Total;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoDolar:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == sale.SaleCondition).FirstOrDefault();
                                            box.Sold -= sale.Total;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoReal:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == sale.SaleCondition).FirstOrDefault();
                                            box.Sold -= sale.Total;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoEuro:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == sale.SaleCondition).FirstOrDefault();
                                            box.Sold -= sale.Total;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.EfectivoOtro:
                                        {
                                            Box box = StaticBoxAndBank.Boxes.Where(b => b.SaleCondition == sale.SaleCondition).FirstOrDefault();
                                            box.Sold -= sale.Total;
                                            box.UpdateDate = DateTime.Now;
                                            box.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(box);
                                            break;
                                        }
                                    case SaleCondition.CtaCte:
                                        {
                                            break;
                                        }
                                    case SaleCondition.Cheque:
                                        {
                                            break;
                                        }
                                    case SaleCondition.MercadoPagoTransf:
                                        {
                                            BankParameter bankParameter = StaticBoxAndBank.BankParameters.Where(bp => bp.SaleCondition == sale.SaleCondition).First();
                                            Bank bank = StaticBoxAndBank.FindBank(bankParameter.BankId);
                                            decimal fromAcredit = sale.Total - (sale.Total * bankParameter.Rate / 100);
                                            bank.Sold -= fromAcredit;
                                            bank.UpdateDate = DateTime.Now;
                                            bank.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(bank);
                                            break;
                                        }
                                    case SaleCondition.Deposito:
                                        {
                                            BankParameter bankParameter = StaticBoxAndBank.BankParameters.Where(bp => bp.SaleCondition == sale.SaleCondition).First();
                                            Bank bank = StaticBoxAndBank.FindBank(bankParameter.BankId);
                                            decimal fromAcredit = sale.Total - (sale.Total * bankParameter.Rate / 100);
                                            bank.Sold -= fromAcredit;
                                            bank.UpdateDate = DateTime.Now;
                                            bank.UpdateUser = UserCache.UserName;
                                            await _generalGestor.UpdateAsync(bank);
                                            break;
                                        }
                                    default:
                                        {
                                            BankParameter bankParameter = StaticBoxAndBank.BankParameters.Where(bp => bp.SaleCondition == sale.SaleCondition).First();
                                            Bank bank = StaticBoxAndBank.FindBank(bankParameter.BankId);

                                            Acreditation acreditation = StaticAcreditation.FindFromSaleId(sale.Id);
                                            if (acreditation != null)
                                            {
                                                acreditation.IsEnabled = false;
                                                acreditation.IsDeleted = false;
                                                acreditation.UpdateDate = DateTime.Now;
                                                acreditation.UpdateUser = UserCache.UserName;

                                                await _generalGestor.UpdateAsync(acreditation);

                                                if (acreditation.IsAcredited)
                                                    bank.Sold -= acreditation.FromAcredit;
                                                else
                                                    bank.FromCredit -= acreditation.FromAcredit;
                                                bank.UpdateDate = DateTime.Now;
                                                bank.UpdateUser = UserCache.UserName;
                                                await _generalGestor.UpdateAsync(bank);
                                            }
                                            break;
                                        }
                                }
                            */

                            client.Sold -= sale.Total;
                            _context.Update(client);

                            GeneralResponse resultSave = await _dBHelper.SaveChangesAsync(_context);
                            if (resultSave.Success)
                            {
                                StaticCommon.ContextInUse = true;
                                transacction.Commit();
                                StaticCommon.ContextInUse = false;
                                return new SaleResponse { Success = true, SaleId = newSale.Id };
                            }
                            else
                            {
                                transacction.Rollback();
                                return new SaleResponse { Success = false, Message = resultSave.Message };
                            }
                        }
                        else
                        {
                            transacction.Rollback();
                            return new SaleResponse { Success = false, Message = resultAdd.Message };
                        }
                    }
                    else
                    {
                        transacction.Rollback();
                        return new SaleResponse { Success = false, Message = "No se localiza el comprobante a anular" };
                    }
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

                // 👇 Traigo las facturas asociadas a las ventas que obtuve
                List<int>? saleIds = sales.Select(s => s.Id).ToList();
                List<Invoice>? invoices = await _context.Invoices
                    .Where(i => saleIds.Contains(i.SaleId))
                    .ToListAsync();

                return new SaleResponse
                {
                    Success = true,
                    SaleViewModels = ToSaleViewModelsList(sales, invoices),
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

                // 👇 Traigo las facturas asociadas a las ventas que obtuve
                List<int>? saleIds = sales.Select(s => s.Id).ToList();
                List<Invoice>? invoices = await _context.Invoices
                    .Where(i => saleIds.Contains(i.SaleId))
                    .ToListAsync();



                return new SaleResponse
                {
                    Success = true,
                    SaleViewModels = ToSaleViewModelsList(sales, invoices),
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


                if (id == 0 || sale == null)
                    return new SaleResponse
                    {
                        Success = true,
                        SaleViewModel = new SaleViewModel(),
                    };

                // 👇 Traigo la factura asociada a las venta que obtuve
                Invoice? invoice = await _context.Invoices
                     .Where(i => i.SaleId == sale.Id)
                     .FirstAsync();


                return new SaleResponse
                {
                    Success = true,
                    SaleViewModel = ConverterHelper.ToSaleViewModel(sale, invoice),
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




        private List<SaleViewModel> ToSaleViewModelsList(List<Sale> sales, List<Invoice> invoices)
        {
            List<SaleViewModel> saleViewModels = sales.Select(sale =>
            {
                Invoice? invoice = invoices.FirstOrDefault(i => i.SaleId == sale.Id);
                DateTime? invoiceDate = null;
                if (invoice != null)
                    invoiceDate = DateTime.ParseExact(invoice?.InvoiceDate, "yyyyMMdd", CultureInfo.InvariantCulture).Date;

                return new SaleViewModel
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
                    TotalIVA25 = sale.TotalIVA25,
                    BaseImp5 = sale.BaseImp5,
                    BaseImp0 = sale.BaseImp0,
                    BaseImp25 = sale.BaseImp25,
                    TotalIVA5 = sale.TotalIVA5,
                    InvoiceNumber = invoice?.NumberString,
                    InvoiceDate = invoiceDate,
                    HasCAE = invoice != null && invoice?.CAE != string.Empty,
                    //Clients = clients,
                    //SaleConditions = saleConditions,
                    //PriceLists = priceLists,
                    //SaleCondition = sale.SaleCondition,
                    //SaleConditionId = sale.SaleConditionId,


                };
            })
                .OrderBy(s => s.SalePoint)
                .ThenBy(s => s.SaleNumber)
                .ToList();

            return saleViewModels;
        }


    }
}
