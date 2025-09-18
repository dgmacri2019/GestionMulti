using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Response;
using GestionComercial.Domain.Statics;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

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
            List<State> states = await _context.States
                .AsNoTracking()
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Name)
                .ToListAsync();

            List<Account> accounts = await _context.Accounts
                .AsNoTracking()
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Name)
                .ToListAsync();

            states.Add(new State { Id = 0, Name = "Seleccione la provincia" });
            accounts.Add(new Account { Id = 0, Name = "Seleccione la cuanta contable" });

            if (id == 0)
                return new BankViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                    Accounts = accounts,
                    States = states,
                };
            else
            {
                Bank? bank = await _context.Banks
                    .AsNoTracking()
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
                    Accounts = accounts,
                    States = states,
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
                .AsNoTracking()
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Description)
                .ToListAsync();

            List<Account> accounts = await _context.Accounts
                .AsNoTracking()
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Name)
                .ToListAsync();

            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
            accounts.Add(new Account { Id = 0, Name = "Seleccione la cuanta contable" });

            if (id == 0)
                return new BoxViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                    Accounts = accounts,
                    SaleConditions = saleConditions,
                };
            else
            {
                Box? box = await _context.Boxes
                    .AsNoTracking()
                    .Where(b => b.Id == id)
                    .Include(a => a.Account)
                    //.Include(sc => sc.SaleCondition)
                    .FirstOrDefaultAsync();

                return new BoxViewModel
                {
                    UpdateUser = box.UpdateUser,
                    CreateUser = box.CreateUser,
                    Accounts = accounts,
                    UpdateDate = box.UpdateDate,
                    AccountId = box.AccountId,
                    BoxName = box.BoxName,
                    CreateDate = box.CreateDate,
                    FromCredit = box.FromCredit,
                    FromDebit = box.FromDebit,
                    Id = box.Id,
                    IsDeleted = box.IsDeleted,
                    IsEnabled = box.IsEnabled,
                    //SaleConditionId = box.SaleConditionId,
                    Sold = box.Sold,
                    SaleConditions = saleConditions,
                };
            }
        }
        public async Task<IEnumerable<BankAndBoxViewModel>> SearchBankAndBoxToListAsync()
        {
            List<BankAndBoxViewModel> bankAndBoxes = [];

            while (StaticCommon.ContextInUse)
                await Task.Delay(100);
            StaticCommon.ContextInUse = true;

            List<Bank> banks = await _context.Banks
                .AsNoTracking()
                .Include(s => s.State)
                .Include(bp => bp.BankParameters)
                .Include(a => a.Acreditations)
                .Include(d => d.Debitations)
                .ToListAsync();


            List<Box> boxes = await _context.Boxes
                .AsNoTracking()
                .Include(a => a.Account)
                //.Include(sc => sc.SaleCondition)
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
                    //SaleConditionId = b.SaleConditionId,
                    AccountId = b.AccountId,
                    Sold = b.Sold,
                    IsBank = false,
                   // SaleConditionString = b.SaleCondition.Description,
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
                    Address = b.Address,
                    PostalCode = b.PostalCode,
                    StateId = b.StateId,
                    City = b.City,
                    Phone = b.Phone,
                    Phone1 = b.Phone1,
                    Email = b.Email,
                    WebSite = b.WebSite,
                    AccountNumber = b.AccountNumber,
                    CBU = b.CBU,
                    Alias = b.Alias,
                    AccountId = b.AccountId,
                });
            return bankAndBoxes.ToList();
        }

        public async Task<IEnumerable<BankParameterViewModel>> SearchBankParameterToListAsync()
        {
            List<BankParameterViewModel> bankParameterViewModels = [];

            while (StaticCommon.ContextInUse)
                await Task.Delay(100);
            StaticCommon.ContextInUse = true;

            List<Bank> banks = await _context.Banks
                .AsNoTracking()
                .Where(b => b.IsEnabled && !b.IsDeleted)
                .Include(a => a.Account)
                .Include(s => s.State)
                .OrderBy(b => b.BankName)
                .ToListAsync();

            List<BankParameter> bankParameters = await _context.BankParameters
                .AsNoTracking()
                .Where(a => a.IsEnabled && !a.IsDeleted)
                .Include(s => s.Bank)
                //.Include(sc => sc.SaleCondition)
                .ToListAsync();

            List<SaleCondition> saleConditions = await _context.SaleConditions
                .AsNoTracking()
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
                .AsNoTracking()
                .Where(sc => sc.IsEnabled && !sc.IsDeleted)
                .OrderBy(sc => sc.Description)
                .ToListAsync();
            List<Bank> banks = await _context.Banks
                .AsNoTracking()
               .Where(b => b.IsEnabled && !b.IsDeleted)
               .Include(a => a.Account)
               .Include(s => s.State)
               .OrderBy(b => b.BankName)
               .ToListAsync();

            saleConditions.Add(new SaleCondition { Id = 0, Description = "Seleccione la condición de venta" });
            banks.Add(new Bank { Id = 0, BankName = "Seleccione el banco" });

            if (id == 0)
                return new BankParameterViewModel
                {
                    IsDeleted = false,
                    IsEnabled = true,
                    CreateDate = DateTime.Now,
                    Banks = banks,
                    SaleConditions = saleConditions,
                };
            else
            {
                BankParameter? bankParameter = await _context.BankParameters
                    .AsNoTracking()
                 .Where(b => b.Id == id)
                 .Include(a => a.Bank)
                // .Include(sc => sc.SaleCondition)
                 .FirstOrDefaultAsync();

                return new BankParameterViewModel
                {
                    Banks = banks,
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
                    //SaleConditionId = bankParameter.SaleConditionId,
                    BankName = bankParameter.Bank.BankName,
                   // SaleConditionString = bankParameter.SaleCondition.Description,
                };
            }
        }





        private IEnumerable<BankParameterViewModel> ToBankParameterViewModelList(List<BankParameter> bankParameters, List<Bank> banks, List<SaleCondition> saleConditions)
        {
            return bankParameters.Select(bankParameter => new BankParameterViewModel
            {
                Id = bankParameter.Id,
                //SaleConditionId = bankParameter.SaleConditionId,
                CreateDate = bankParameter.CreateDate,
                CreateUser = bankParameter.CreateUser,
                UpdateDate = bankParameter.UpdateDate,
                UpdateUser = bankParameter.UpdateUser,
                IsDeleted = bankParameter.IsDeleted,
                IsEnabled = bankParameter.IsEnabled,
                Banks = banks,
                BankName = banks.Where(b => b.Id == bankParameter.BankId).FirstOrDefault().BankName,
                SaleConditions = saleConditions,
                AcreditationDay = bankParameter.AcreditationDay,
                BankId = bankParameter.BankId,
                DebitationDay = bankParameter.DebitationDay,
                Rate = bankParameter.Rate,
                //SaleConditionString = bankParameter.SaleCondition.Description,
            });
        }
    }
}
