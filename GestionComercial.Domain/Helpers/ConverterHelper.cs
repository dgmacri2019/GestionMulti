using GestionComercial.Domain.Constant;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.DTOs.Provider;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.DTOs.User;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Entities.Stock;
using System.Collections.ObjectModel;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Domain.Helpers
{
    public static class ConverterHelper
    {
        #region Stocks

        public static Article ToArticle(ArticleViewModel articleViewModel, bool isNew)
        {
            return new Article
            {
                Id = isNew ? 0 : articleViewModel.Id,
                BarCode = articleViewModel.BarCode,
                UpdateUser = articleViewModel.UpdateUser,
                UpdateDate = articleViewModel.UpdateDate,
                Umbral = articleViewModel.Umbral,
                TaxId = articleViewModel.TaxId,
                StockCheck = articleViewModel.StockCheck,
                Stock = articleViewModel.Stock,
                Bonification = articleViewModel.Bonification / 100,
                CategoryId = articleViewModel.CategoryId,
                ChangePoint = articleViewModel.ChangePoint,
                Clarifications = articleViewModel.Clarifications,
                Code = articleViewModel.Code,
                Cost = articleViewModel.Cost,
                CreateDate = DateTime.Now,
                CreateUser = articleViewModel.CreateUser,
                Description = articleViewModel.Description,
                InternalTax = articleViewModel.InternalTax,
                IsDeleted = articleViewModel.IsDeleted,
                IsEnabled = articleViewModel.IsEnabled,
                IsWeight = articleViewModel.IsWeight,
                MeasureId = articleViewModel.MeasureId,
                MinimalStock = articleViewModel.MinimalStock,
                RealCost = articleViewModel.RealCost,
                Remark = articleViewModel.Remark,
                Replacement = articleViewModel.Replacement,
                SalePoint = articleViewModel.SalePoint,
            };
        }

        public static ArticleViewModel ToArticleViewModel(Article? article, ICollection<Tax> taxes, ICollection<Measure> measures, ICollection<Category> categories)
        {
            return new ArticleViewModel
            {
                Id = article.Id,
                BarCode = article.BarCode,
                Bonification = article.Bonification * 100,
                CategoryId = article.CategoryId,
                ChangePoint = article.ChangePoint,
                Clarifications = article.Clarifications,
                Code = article.Code,
                Cost = article.Cost,
                CreateDate = article.CreateDate,
                CreateUser = article.CreateUser,
                Description = article.Description,
                InternalTax = article.InternalTax,
                IsDeleted = article.IsDeleted,
                IsEnabled = article.IsEnabled,
                IsWeight = article.IsWeight,
                MeasureId = article.MeasureId,
                MinimalStock = article.MinimalStock,
                RealCost = article.RealCost,
                Remark = article.Remark,
                Replacement = article.Replacement,
                SalePoint = article.SalePoint,
                Stock = article.Stock,
                StockCheck = article.StockCheck,
                TaxId = article.TaxId,
                Umbral = article.Umbral,
                UpdateDate = article.UpdateDate,
                UpdateUser = article.UpdateUser,
                Categories = categories,
                Measures = measures,
                Taxes = taxes,
            };
        }

        #endregion

        #region Clients

        public static Client ToClient(ClientViewModel clientViewModel, bool isNew)
        {
            return new Client
            {
                Id = isNew ? 0 : clientViewModel.Id,
                Address = clientViewModel.Address,
                City = clientViewModel.City,
                WebSite = clientViewModel.WebSite,
                BusinessName = clientViewModel.BusinessName,
                OptionalCode = clientViewModel.OptionalCode,
                CreateDate = clientViewModel.CreateDate,
                CreateUser = clientViewModel.CreateUser,
                DocumentNumber = clientViewModel.DocumentNumber,
                DocumentTypeId = clientViewModel.DocumentTypeId,
                Email = clientViewModel.Email,
                FantasyName = clientViewModel.FantasyName,
                IsDeleted = clientViewModel.IsDeleted,
                IsEnabled = clientViewModel.IsEnabled,
                LastPuchase = clientViewModel.LastPuchase,
                LegendBudget = clientViewModel.LegendBudget,
                LegendInvoices = clientViewModel.LegendInvoices,
                LegendOrder = clientViewModel.LegendOrder,
                LegendRemit = clientViewModel.LegendRemit,
                PayDay = clientViewModel.PayDay,
                Phone = clientViewModel.Phone,
                Phone1 = clientViewModel.Phone1,
                Phone2 = clientViewModel.Phone2,
                PostalCode = clientViewModel.PostalCode,
                PriceListId = clientViewModel.PriceListId,
                Remark = clientViewModel.Remark,
                SaleConditionId = clientViewModel.SaleConditionId,
                Sold = clientViewModel.Sold,
                StateId = clientViewModel.StateId,
                IvaConditionId = clientViewModel.IvaConditionId,
                UpdateDate = clientViewModel.UpdateDate,
                UpdateUser = clientViewModel.UpdateUser,
            };
        }

        public static ClientViewModel ToClientViewModel(Client client, ICollection<PriceList> priceLists,
            ICollection<State> states, ICollection<SaleCondition> saleConditions, ICollection<IvaCondition> ivaConditions,
            ICollection<DocumentType> documentTypes)
        {
            return new ClientViewModel
            {
                Address = client.Address,
                BusinessName = client.BusinessName,
                OptionalCode = client.OptionalCode,
                City = client.City,
                DocumentNumber = client.DocumentNumber,
                DocumentTypeId = client.DocumentTypeId,
                DocumentTypes = documentTypes,
                DocumentTypeString = client.DocumentType.Description,
                Email = client.Email,
                FantasyName = client.FantasyName,
                Id = client.Id,
                LastPuchase = client.LastPuchase,
                LegendBudget = client.LegendBudget,
                LegendInvoices = client.LegendInvoices,
                LegendOrder = client.LegendOrder,
                LegendRemit = client.LegendRemit,
                PayDay = client.PayDay,
                Phone = client.Phone,
                Phone1 = client.Phone1,
                Phone2 = client.Phone2,
                PostalCode = client.PostalCode,
                PriceListId = client.PriceListId,
                PriceLists = priceLists,
                Remark = client.Remark,
                SaleConditionId = client.SaleConditionId,
                SaleConditions = saleConditions,
                SaleConditionString = client.SaleCondition.Description,
                Sold = client.Sold,
                StateId = client.StateId,
                States = states,
                IvaConditionId = client.IvaConditionId,
                IvaConditions = ivaConditions,
                IvaConditionString = client.IvaCondition.Description,
                WebSite = client.WebSite,
                CreateUser = client.CreateUser,
                CreateDate = client.CreateDate,
                UpdateUser = client.UpdateUser,
                UpdateDate = client.UpdateDate,
                IsEnabled = client.IsEnabled,
                IsDeleted = client.IsDeleted,
                PriceList = client.PriceList.Description,
                State = client.State.Name,

            };
        }


        #endregion

        #region Providers

        public static Provider ToProvider(ProviderViewModel providerViewModel, bool isNew)
        {
            return new Provider
            {
                Id = isNew ? 0 : providerViewModel.Id,
                Address = providerViewModel.Address,
                City = providerViewModel.City,
                WebSite = providerViewModel.WebSite,
                BusinessName = providerViewModel.BusinessName,
                OptionalCode = providerViewModel.OptionalCode,
                CreateDate = providerViewModel.CreateDate,
                CreateUser = providerViewModel.CreateUser,
                DocumentNumber = providerViewModel.DocumentNumber,
                DocumentTypeId = providerViewModel.DocumentTypeId,
                Email = providerViewModel.Email,
                FantasyName = providerViewModel.FantasyName,
                IsDeleted = providerViewModel.IsDeleted,
                IsEnabled = providerViewModel.IsEnabled,
                LastPuchase = providerViewModel.LastPuchase,
                PayDay = providerViewModel.PayDay,
                Phone = providerViewModel.Phone,
                Phone1 = providerViewModel.Phone1,
                Phone2 = providerViewModel.Phone2,
                PostalCode = providerViewModel.PostalCode,
                Remark = providerViewModel.Remark,
                SaleConditionId = providerViewModel.SaleConditionId,
                Sold = providerViewModel.Sold,
                StateId = providerViewModel.StateId,
                IvaConditionId = providerViewModel.IvaConditionId,
                UpdateDate = providerViewModel.UpdateDate,
                UpdateUser = providerViewModel.UpdateUser,
            };
        }

        public static ProviderViewModel? ToProviderViewModel(Provider provider, ICollection<State> states,
            ICollection<SaleCondition> saleConditions, ICollection<IvaCondition> ivaConditions,
             ICollection<DocumentType> documentTypes)
        {
            return new ProviderViewModel
            {
                OptionalCode = provider.OptionalCode,
                Address = provider.Address,
                BusinessName = provider.BusinessName,
                City = provider.City,
                DocumentNumber = provider.DocumentNumber,
                DocumentTypeId = provider.DocumentTypeId,
                DocumentTypes = documentTypes,
                Email = provider.Email,
                FantasyName = provider.FantasyName,
                Id = provider.Id,
                LastPuchase = provider.LastPuchase,
                PayDay = provider.PayDay,
                Phone = provider.Phone,
                Phone1 = provider.Phone1,
                Phone2 = provider.Phone2,
                PostalCode = provider.PostalCode,
                Remark = provider.Remark,
                SaleConditionId = provider.SaleConditionId,
                SaleConditionString = provider.SaleCondition.Description,
                SaleConditions = saleConditions,
                Sold = provider.Sold,
                StateId = provider.StateId,
                States = states,
                IvaConditionId = provider.IvaConditionId,
                IvaConditions = ivaConditions,
                WebSite = provider.WebSite,
                CreateUser = provider.CreateUser,
                CreateDate = provider.CreateDate,
                UpdateUser = provider.UpdateUser,
                UpdateDate = provider.UpdateDate,
                IsEnabled = provider.IsEnabled,
                IsDeleted = provider.IsDeleted,
                State = provider.State.Name,
                DocumentTypeString = provider.DocumentType.Description,
                IvaConditionString = provider.IvaCondition.Description,
            };
        }

        #endregion

        #region PriceList

        public static PriceList ToPriceList(PriceListViewModel priceListViewModel, bool isNew)
        {
            return new PriceList
            {
                Id = isNew ? 0 : priceListViewModel.Id,
                CreateDate = priceListViewModel.CreateDate,
                Description = priceListViewModel.Description,
                CreateUser = priceListViewModel.CreateUser,
                IsDeleted = priceListViewModel.IsDeleted,
                IsEnabled = priceListViewModel.IsEnabled,
                UpdateDate = priceListViewModel.UpdateDate,
                UpdateUser = priceListViewModel.UpdateUser,
                Utility = priceListViewModel.Utility,
            };
        }

        public static PriceListViewModel ToPriceListViewModel(PriceList priceList)
        {
            return new PriceListViewModel
            {
                CreateDate = priceList.CreateDate,
                Description = priceList.Description,
                CreateUser = priceList.CreateUser,
                Id = priceList.Id,
                IsDeleted = priceList.IsDeleted,
                IsEnabled = priceList.IsEnabled,
                UpdateDate = priceList.UpdateDate,
                UpdateUser = priceList.UpdateUser,
                Utility = priceList.Utility,
            };
        }

        #endregion

        #region Bank And Box

        public static Bank ToBank(BankViewModel bankViewModel, bool isNew)
        {
            return new Bank
            {
                Id = isNew ? 0 : bankViewModel.Id,
                CreateDate = bankViewModel.CreateDate,
                CreateUser = bankViewModel.CreateUser,
                IsDeleted = bankViewModel.IsDeleted,
                IsEnabled = bankViewModel.IsEnabled,
                UpdateDate = bankViewModel.UpdateDate,
                UpdateUser = bankViewModel.UpdateUser,
                AccountId = bankViewModel.AccountId,
                AccountNumber = bankViewModel.AccountNumber,
                Address = bankViewModel.Address,
                Alias = bankViewModel.Alias,
                BankName = bankViewModel.BankName,
                CBU = bankViewModel.CBU,
                City = bankViewModel.City,
                Email = bankViewModel.Email,
                FromCredit = bankViewModel.FromCredit,
                FromDebit = bankViewModel.FromDebit,
                Phone = bankViewModel.Phone,
                Phone1 = bankViewModel.Phone1,
                PostalCode = bankViewModel.PostalCode,
                Sold = bankViewModel.Sold,
                StateId = bankViewModel.StateId,
                WebSite = bankViewModel.WebSite
            };
        }

        public static Box ToBox(BoxViewModel boxViewModel, bool isNew)
        {
            return new Box
            {
                Id = isNew ? 0 : boxViewModel.Id,
                CreateDate = boxViewModel.CreateDate,
                CreateUser = boxViewModel.CreateUser,
                IsDeleted = boxViewModel.IsDeleted,
                IsEnabled = boxViewModel.IsEnabled,
                UpdateDate = boxViewModel.UpdateDate,
                UpdateUser = boxViewModel.UpdateUser,
                AccountId = boxViewModel.AccountId,
                BoxName = boxViewModel.BoxName,
                FromCredit = boxViewModel.FromCredit,
                FromDebit = boxViewModel.FromDebit,
                SaleConditionId = boxViewModel.SaleConditionId,
                Sold = boxViewModel.Sold,
            };
        }

        public static BankParameter ToBankParameter(BankParameterViewModel bankParameterViewModel, bool isNew)
        {
            return new BankParameter
            {
                Id = isNew ? 0 : bankParameterViewModel.Id,
                AcreditationDay = bankParameterViewModel.AcreditationDay,
                BankId = bankParameterViewModel.BankId,
                CreateDate = bankParameterViewModel.CreateDate,
                CreateUser = bankParameterViewModel.CreateUser,
                IsDeleted = bankParameterViewModel.IsDeleted,
                IsEnabled = bankParameterViewModel.IsEnabled,
                UpdateDate = bankParameterViewModel.UpdateDate,
                UpdateUser = bankParameterViewModel.UpdateUser,
                DebitationDay = bankParameterViewModel.DebitationDay,
                Rate = bankParameterViewModel.Rate,
                SaleConditionId = bankParameterViewModel.SaleConditionId,
            };
        }


        #endregion

        #region Accounting Books

        public static AccountViewModel ToAccountViewModel(Account account)
        {
            return new AccountViewModel
            {
                Id = account.Id,
                Description = account.Description,
                Name = account.Name,
                CreateDate = account.CreateDate,
                CreateUser = account.CreateUser,
                UpdateDate = account.UpdateDate,
                UpdateUser = account.UpdateUser,
                IsDeleted = account.IsDeleted,
                IsEnabled = account.IsEnabled,
                ForeignCurrency = account.ForeignCurrency,
                IsReference = account.IsReference,
                //AccountGroupNumber = account.AccountGroupNumber,
                AccountTypeId = account.AccountTypeId,
                AccountSubGroupNumber1 = account.AccountSubGroupNumber1,
                AccountSubGroupNumber2 = account.AccountSubGroupNumber2,
                AccountSubGroupNumber3 = account.AccountSubGroupNumber3,
                AccountSubGroupNumber4 = account.AccountSubGroupNumber4,
                AccountSubGroupNumber5 = account.AccountSubGroupNumber5,
                AccountIdSubGroupNumber1 = account.AccountIdSubGroupNumber1,
                AccountIdSubGroupNumber2 = account.AccountIdSubGroupNumber2,
                AccountIdSubGroupNumber3 = account.AccountIdSubGroupNumber3,
                AccountIdSubGroupNumber4 = account.AccountIdSubGroupNumber4,
                IsFirstLevel = account.AccountSubGroupNumber1 == 0 && account.AccountSubGroupNumber2 == 0 && account.AccountSubGroupNumber3 == 0 &&
                account.AccountSubGroupNumber4 == 0 && account.AccountSubGroupNumber5 == 0,
            };
        }

        public static Account ToAccount(AccountViewModel accountViewModel, bool isNew)
        {
            return new Account
            {
                Id = isNew ? 0 : accountViewModel.Id,
                //AccountGroupNumber = accountViewModel.AccountGroupNumber,
                AccountTypeId = accountViewModel.AccountTypeId,
                AccountSubGroupNumber1 = accountViewModel.AccountSubGroupNumber1,
                AccountSubGroupNumber2 = accountViewModel.AccountSubGroupNumber2,
                AccountSubGroupNumber3 = accountViewModel.AccountSubGroupNumber3,
                AccountSubGroupNumber4 = accountViewModel.AccountSubGroupNumber4,
                AccountSubGroupNumber5 = accountViewModel.AccountSubGroupNumber5,
                AccountIdSubGroupNumber1 = accountViewModel.AccountIdSubGroupNumber1,
                AccountIdSubGroupNumber2 = accountViewModel.AccountIdSubGroupNumber2,
                AccountIdSubGroupNumber3 = accountViewModel.AccountIdSubGroupNumber3,
                AccountIdSubGroupNumber4 = accountViewModel.AccountIdSubGroupNumber4,
                IsReference = accountViewModel.IsReference,
                ForeignCurrency = accountViewModel.ForeignCurrency,
                CreateDate = accountViewModel.CreateDate,
                CreateUser = accountViewModel.CreateUser,
                Description = accountViewModel.Description,
                IsDeleted = accountViewModel.IsDeleted,
                IsEnabled = accountViewModel.IsEnabled,
                Name = accountViewModel.Name,
                UpdateDate = accountViewModel.UpdateDate,
                UpdateUser = accountViewModel.UpdateUser,
            };
        }



        #endregion

        #region Users

        public static UserViewModel ToUserViewModel(User user)
        {
            return new UserViewModel
            {
                FirstName = user.FirstName,
                Id = user.Id,
                LastName = user.LastName,
                UserName = user.UserName,
                ChangePassword = user.ChangePassword,
                Email = user.Email,
                Enabled = user.Enabled,
                FullName = user.FullName,
                Phone = user.PhoneNumber,
            };
        }



        #endregion

        #region Sales
        public static SaleViewModel? ToSaleViewModel(Sale sale, ICollection<Client> clients, ICollection<SaleCondition> saleConditions, ICollection<PriceList> priceLists)
        {
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
            };
        }

        public static Sale ToSale(SaleViewModel saleViewModel, bool isNew)
        {
            return new Sale
            {
                Id = isNew ? 0 : saleViewModel.Id,
                CreateDate = saleViewModel.CreateDate,
                CreateUser = saleViewModel.CreateUser,
                IsDeleted = saleViewModel.IsDeleted,
                IsEnabled = saleViewModel.IsEnabled,
                UpdateDate = saleViewModel.UpdateDate,
                UpdateUser = saleViewModel.UpdateUser,
                Acreditations = saleViewModel.Acreditations,
                AutorizationCode = saleViewModel.AutorizationCode,
                BaseImp105 = saleViewModel.BaseImp105,
                BaseImp21 = saleViewModel.BaseImp21,
                BaseImp27 = saleViewModel.BaseImp27,
                ClientId = saleViewModel.ClientId,
                GeneralDiscount = saleViewModel.GeneralDiscount,
                InternalTax = saleViewModel.InternalTax,
                IsFinished = saleViewModel.IsFinished,
                PaidOut = saleViewModel.PaidOut,
                PartialPay = saleViewModel.PartialPay,
                SaleConditionId = saleViewModel.SaleConditionId,
                SaleDate = saleViewModel.SaleDate,
                SaleDetails = saleViewModel.SaleDetails,
                SaleNumber = saleViewModel.SaleNumber,
                SalePayMetodDetails = saleViewModel.SalePayMetodDetails,
                SalePoint = saleViewModel.SalePoint,
                Sold = saleViewModel.Sold,
                SubTotal = saleViewModel.SubTotal,
                Total = saleViewModel.Total,
                TotalIVA105 = saleViewModel.TotalIVA105,
                TotalIVA21 = saleViewModel.TotalIVA21,
                TotalIVA27 = saleViewModel.TotalIVA27,
            };
        }



        #endregion
    }
}
