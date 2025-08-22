using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Applications.Services
{
    public class BankService : IBankService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;

        #region Contructor

        public BankService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();

        }

        #endregion


        public Task<GeneralResponse> DeleteAcreditationAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteBankAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteBankParamerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteBoxAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<GeneralResponse> DeleteDebitationAsync(int id)
        {
            throw new NotImplementedException();
        }


        public async Task<BankViewModel?> GetBankByIdAsync(int id)
        {
            if (id == 0)
                return new BankViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                    Accounts = await _context.Accounts.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync(),
                    States = await _context.States.Where(s => s.IsEnabled && !s.IsDeleted).ToListAsync(),
                };
            else
            {
                Bank? bank = await _context.Banks
                 .Where(b => b.Id == id)
                 .Include(a => a.Account)
                 .Include(s => s.State)
                 .Include(bp => bp.BankParameters)
                 .Include(a => a.Acreditations)
                 .Include(d => d.Debitations)
                 .FirstOrDefaultAsync();
                return new BankViewModel
                {
                    AccountId = bank.AccountId,
                    AccountNumber = bank.AccountNumber,
                    Accounts = await _context.Accounts.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync(),
                    States = await _context.States.Where(s => s.IsEnabled && !s.IsDeleted).ToListAsync(),
                    Address = bank.Address,
                    Alias = bank.Alias,
                    BankName = bank.BankName,
                    CBU = bank.CBU,
                    City = bank.City,
                    CreateDate = bank.CreateDate,
                    CreateUser = bank.CreateUser,
                    Email = bank.Email,
                    Id = bank.Id,
                    IsDeleted = bank.IsDeleted,
                    IsEnabled = bank.IsEnabled,
                    Phone = bank.Phone,
                    Phone1 = bank.Phone1,
                    PostalCode = bank.PostalCode,
                    Sold = bank.Sold,
                    UpdateDate = bank.UpdateDate,
                    UpdateUser = bank.UpdateUser,
                    WebSite = bank.WebSite,
                    FromDebit = bank.FromDebit,
                    FromCredit = bank.FromCredit,
                    StateId = bank.StateId,
                };
            }
        }
        public async Task<BoxViewModel?> GetBoxByIdAsync(int id)
        {
            List<SaleCondition> saleConditions = await _context.SaleConditions
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Description)
                .ToListAsync();
            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });

            if (id == 0)
                return new BoxViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                    Accounts = await _context.Accounts.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync(),
                    SaleConditions = saleConditions,
                };
            else
            {
                Box? box = await _context.Boxes
                .Where(b => b.Id == id)
                .Include(a => a.Account)
                .Include(sc => sc.SaleCondition)
                .FirstOrDefaultAsync();

                return new BoxViewModel
                {
                    UpdateUser = box.UpdateUser,
                    CreateUser = box.CreateUser,
                    Accounts = await _context.Accounts.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync(),
                    UpdateDate = box.UpdateDate,
                    AccountId = box.AccountId,
                    BoxName = box.BoxName,
                    CreateDate = box.CreateDate,
                    FromCredit = box.FromCredit,
                    FromDebit = box.FromDebit,
                    Id = box.Id,
                    IsDeleted = box.IsDeleted,
                    IsEnabled = box.IsEnabled,
                    SaleConditionId = box.SaleConditionId,
                    Sold = box.Sold,
                    SaleConditions = saleConditions,
                };
            }
        }
        public async Task<IEnumerable<BankAndBoxViewModel>> SearchBankAndBoxToListAsync(string name, bool isEnabled, bool isDeleted)
        {
            List<BankAndBoxViewModel> bankAndBoxes = [];

            while (StaticCommon.ContextInUse)
                await Task.Delay(100);
            StaticCommon.ContextInUse = true;

            List<Bank> banks = string.IsNullOrEmpty(name) ?
                await _context.Banks
                .Where(b => b.IsEnabled == isEnabled && b.IsDeleted == isDeleted)
                .Include(s => s.State)
                .Include(bp => bp.BankParameters)
                .Include(a => a.Acreditations)
                .Include(d => d.Debitations)
                .ToListAsync()
                :
                await _context.Banks
                .Where(b => b.IsEnabled == isEnabled && b.IsDeleted == isDeleted && b.BankName.Contains(name))
                .Include(s => s.State)
                .Include(bp => bp.BankParameters)
                .Include(a => a.Acreditations)
                .Include(d => d.Debitations)
                .ToListAsync();

            List<Box> boxes = string.IsNullOrEmpty(name) ?
                await _context.Boxes
                .Where(b => b.IsEnabled == isEnabled && b.IsDeleted == isDeleted)
                .Include(a => a.Account)
                .Include(sc => sc.SaleCondition)
                .ToListAsync()
                :
                await _context.Boxes
                .Where(b => b.IsEnabled == isEnabled && b.IsDeleted == isDeleted && b.BoxName.Contains(name))
                .Include(a => a.Account)
                .Include(sc => sc.SaleCondition)
                .ToListAsync();
            StaticCommon.ContextInUse = false;

            foreach (Box b in boxes)
                bankAndBoxes.Add(new BankAndBoxViewModel
                {
                    Id = b.Id,
                    IsDeleted = b.IsDeleted,
                    IsEnabled = b.IsEnabled,
                    BankName = b.BoxName,
                    FromCredit = b.FromCredit,
                    FromDebit = b.FromDebit,
                    Sold = b.Sold,
                    IsBank = false,
                });
            foreach (Bank b in banks)
                bankAndBoxes.Add(new BankAndBoxViewModel
                {
                    Id = b.Id,
                    IsDeleted = b.IsDeleted,
                    IsEnabled = b.IsEnabled,
                    BankName = b.BankName,
                    FromCredit = b.FromCredit,
                    FromDebit = b.FromDebit,
                    Sold = b.Sold,
                    IsBank = true,
                });
            return bankAndBoxes.ToList();
        }

        public async Task<IEnumerable<BankParameterViewModel>> SearchBankParameterToListAsync(string name, bool isEnabled, bool isDeleted)
        {
            List<BankParameterViewModel> bankParameterViewModels = [];

            while (StaticCommon.ContextInUse)
                await Task.Delay(100);
            StaticCommon.ContextInUse = true;

            List<Bank> banks = await _context.Banks.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync();

            List<BankParameter> bankParameters = string.IsNullOrEmpty(name) ?
                await _context.BankParameters
                .Where(b => b.IsEnabled == isEnabled && b.IsDeleted == isDeleted)
                .Include(s => s.Bank)
                .Include(sc => sc.SaleCondition)
                .ToListAsync()
                :
                await _context.BankParameters
                .Include(s => s.Bank)
                .Include(sc=>sc.SaleCondition)
                .Where(b => b.IsEnabled == isEnabled && b.IsDeleted == isDeleted && b.Bank.BankName.Contains(name))
                .ToListAsync();

            List<SaleCondition> saleConditions = await _context.SaleConditions
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Description)
                .ToListAsync();
            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });

            StaticCommon.ContextInUse = false;


            return ToBankParameterViewModelList(bankParameters, banks, saleConditions);
        }

        public async Task<BankParameterViewModel?> GetBankParameterByIdAsync(int id)
        {
            List<SaleCondition> saleConditions = await _context.SaleConditions
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Description)
                .ToListAsync();
            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });

            if (id == 0)
                return new BankParameterViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                    Banks = await _context.Banks.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync(),
                    SaleConditions = saleConditions,
                };
            else
            {
                BankParameter? bankParameter = await _context.BankParameters
                 .Where(b => b.Id == id)
                 .Include(a => a.Bank)
                 .Include(sc => sc.SaleCondition)
                 .FirstOrDefaultAsync();



                return new BankParameterViewModel
                {
                    Banks = await _context.Banks.Where(a => a.IsEnabled && !a.IsDeleted).ToListAsync(),
                    CreateDate = bankParameter.CreateDate,
                    CreateUser = bankParameter.CreateUser,
                    Id = bankParameter.Id,
                    IsDeleted = bankParameter.IsDeleted,
                    IsEnabled = bankParameter.IsEnabled,
                    UpdateDate = bankParameter.UpdateDate,
                    UpdateUser = bankParameter.UpdateUser,
                    SaleConditions = saleConditions,
                    AcreditationDay = bankParameter.AcreditationDay,
                    BankId = bankParameter.BankId,
                    DebitationDay = bankParameter.DebitationDay,
                    Rate = bankParameter.Rate,
                    SaleConditionId = bankParameter.SaleConditionId,
                    BankName = bankParameter.Bank.BankName,
                };
            }
        }





        private IEnumerable<BankParameterViewModel> ToBankParameterViewModelList(List<BankParameter> bankParameters, List<Bank> banks, List<SaleCondition> saleConditions)
        {
            return bankParameters.Select(provider => new BankParameterViewModel
            {
                Id = provider.Id,
                SaleConditionId = provider.SaleConditionId,
                CreateDate = provider.CreateDate,
                CreateUser = provider.CreateUser,
                UpdateDate = provider.UpdateDate,
                UpdateUser = provider.UpdateUser,
                IsDeleted = provider.IsDeleted,
                IsEnabled = provider.IsEnabled,
                Banks = banks,
                BankName = banks.Where(b => b.Id == provider.BankId).FirstOrDefault().BankName,
                SaleConditions = saleConditions,
                AcreditationDay = provider.AcreditationDay,
                BankId = provider.BankId,
                DebitationDay = provider.DebitationDay,
                Rate = provider.Rate,
            });
        }
    }
}
