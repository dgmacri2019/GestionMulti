using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.DTOs.Client;
using GestionComercial.Domain.DTOs.Master.Configurations.Commerce;
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
                Utility = articleViewModel.Utility,
                CostWithTaxes = articleViewModel.CostWithTaxes,
                SalePrice = articleViewModel.SalePrice,
                SalePriceWithTaxes = articleViewModel.SalePriceWithTaxes,
            };
        }

        public static ArticleViewModel ToArticleViewModel(Article? article, List<PriceList> priceLists)
        {
            return new ArticleViewModel
            {
                Id = article.Id,
                Stock = article.Stock,
                Code = article.Code,
                Description = article.Description,
                Category = article.Category.Description,
                CategoryColor = article.Category.Color,
                CostWithTaxes = article.CostWithTaxes,
                Cost = article.Cost,
                Bonification = article.Bonification,
                RealCost = article.RealCost,
                BarCode = article.BarCode,
                CategoryId = article.CategoryId,
                ChangePoint = article.ChangePoint,
                Clarifications = article.Clarifications,
                CreateDate = article.CreateDate,
                CreateUser = article.CreateUser,
                InternalTax = article.InternalTax,
                IsDeleted = article.IsDeleted,
                IsEnabled = article.IsEnabled,
                IsWeight = article.IsWeight,
                MeasureId = article.MeasureId,
                MinimalStock = article.MinimalStock,
                Remark = article.Remark,
                Replacement = article.Replacement,
                SalePoint = article.SalePoint,
                StockCheck = article.StockCheck,
                TaxId = article.TaxId,
                Umbral = article.Umbral,
                UpdateDate = article.UpdateDate,
                UpdateUser = article.UpdateUser,
                Utility = article.Utility,
                SalePrice = article.SalePrice,
                SalePriceWithTaxes = article.SalePriceWithTaxes,
                //Categories = categories,
                //Measures = measures,
                //Taxes = taxes,
                TaxesPrice =
                    [
                        new TaxePriceDto
                        {
                            Description = $"I.V.A. {article.Tax.Description}",
                            Utility = article.Tax.Rate,
                            Price = article.RealCost * article.Tax.Rate /100,
                        },
                        new TaxePriceDto
                        {
                            Description = string.Format("Imp. int. {0}%", article.InternalTax),
                            Utility =article.InternalTax,
                            Price = article.RealCost * article.InternalTax /100,
                        }
                    ],
                PriceLists = priceLists
                    .Select(pl => new PriceListItemDto
                    {
                        Id = pl.Id,
                        Description = pl.Description,
                        Utility = pl.Utility,
                        FinalPrice = article.SalePriceWithTaxes + (article.SalePriceWithTaxes * pl.Utility / 100),
                    })
                    .OrderBy(pl => pl.Utility)
                    .ToList()// Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
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
                CreditLimit = clientViewModel.CreditLimit,
                Sold = clientViewModel.Sold,
                StateId = clientViewModel.StateId,
                IvaConditionId = clientViewModel.IvaConditionId,
                UpdateDate = clientViewModel.UpdateDate,
                UpdateUser = clientViewModel.UpdateUser,
                //SaleConditionId = clientViewModel.SaleConditionId,
            };
        }

        public static ClientViewModel ToClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Address = client.Address,
                BusinessName = client.BusinessName,
                OptionalCode = client.OptionalCode,
                City = client.City,
                DocumentNumber = client.DocumentNumber,
                DocumentTypeId = client.DocumentTypeId,
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
                Remark = client.Remark,
                CreditLimit = client.CreditLimit,
                Sold = client.Sold,
                StateId = client.StateId,
                IvaConditionId = client.IvaConditionId,
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
                //SaleConditionId = client.SaleConditionId,
                //SaleConditionString = client.SaleCondition.Description,
                //DocumentTypes = documentTypes,
                //States = states,
                //SaleConditions = saleConditions,
                //IvaConditions = ivaConditions,
                //PriceLists = priceLists,
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

        #region Masters

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

        #region Category
        public static CategoryViewModel ToCategoryViewModel(Category category)
        {
            return new CategoryViewModel
            {
                Id = category.Id,
                CreateDate = category.CreateDate,
                CreateUser = category.CreateUser,
                IsDeleted = category.IsDeleted,
                IsEnabled = category.IsEnabled,
                UpdateDate = category.UpdateDate,
                UpdateUser = category.UpdateUser,
                Color = category.Color,
                Description = category.Description,
            };
        }
        public static Category ToCategory(CategoryViewModel categoryViewModel, bool isNew)
        {
            return new Category
            {
                Id = isNew ? 0 : categoryViewModel.Id,
                CreateDate = categoryViewModel.CreateDate,
                CreateUser = categoryViewModel.CreateUser,
                IsDeleted = categoryViewModel.IsDeleted,
                IsEnabled = categoryViewModel.IsEnabled,
                UpdateDate = categoryViewModel.UpdateDate,
                UpdateUser = categoryViewModel.UpdateUser,
                Color = categoryViewModel.Color,
                Description = categoryViewModel.Description,
            };
        }

        #endregion


        #region Commerce Data

        public static BillingViewModel? ToBillingViewModel(Billing? billing)
        {

            return billing == null ? null : new BillingViewModel
            {
                Id = billing.Id,
                IsDeleted = billing.IsDeleted,
                IsEnabled = billing.IsEnabled,
                CreateDate = billing.CreateDate,
                CreateUser = billing.CreateUser,
                UpdateDate = billing.UpdateDate,
                UpdateUser = billing.UpdateUser,
                CertPass = billing.CertPass,
                CommerceDataId = billing.CommerceDataId,
                Concept = billing.Concept,
                EmitInvoiceM = billing.EmitInvoiceM,
                ExpireCertificate = billing.ExpireCertificate,
                ExpireCertificateText = billing.ExpireCertificateText,
                HasCertificate = billing.HasCertificate,
                PadronExpirationTime = billing.PadronExpirationTime,
                PadronGenerationTime = billing.PadronGenerationTime,
                PadronSign = billing.PadronSign,
                PadronToken = billing.PadronToken,
                UniqueId = billing.UniqueId,
                UsePadron = billing.UsePadron,
                UseWSDL = billing.UseWSDL,
                WSDLExpirationTime = billing.WSDLExpirationTime,
                WSDLGenerationTime = billing.WSDLGenerationTime,
                WSDLSign = billing.WSDLSign,
                WSDLToken = billing.WSDLToken,
                CertPath = string.Empty,
                UseHomologacion = billing.UseHomologacion
            };
        }
        public static Billing? ToBilling(BillingViewModel? billingViewModel, bool isNew)
        {
            return new Billing
            {
                Id = isNew ? 0 : billingViewModel.Id,
                IsDeleted = billingViewModel.IsDeleted,
                IsEnabled = billingViewModel.IsEnabled,
                CreateDate = billingViewModel.CreateDate,
                CreateUser = billingViewModel.CreateUser,
                UpdateDate = billingViewModel.UpdateDate,
                UpdateUser = billingViewModel.UpdateUser,
                CertPass = billingViewModel.CertPass,
                CommerceDataId = billingViewModel.CommerceDataId,
                Concept = billingViewModel.Concept,
                EmitInvoiceM = billingViewModel.EmitInvoiceM,
                ExpireCertificate = billingViewModel.ExpireCertificate,
                ExpireCertificateText = billingViewModel.ExpireCertificateText,
                HasCertificate = billingViewModel.HasCertificate,
                PadronExpirationTime = billingViewModel.PadronExpirationTime,
                PadronGenerationTime = billingViewModel.PadronGenerationTime,
                PadronSign = billingViewModel.PadronSign,
                PadronToken = billingViewModel.PadronToken,
                UniqueId = billingViewModel.UniqueId,
                UsePadron = billingViewModel.UsePadron,
                UseWSDL = billingViewModel.UseWSDL,
                WSDLExpirationTime = billingViewModel.WSDLExpirationTime,
                WSDLGenerationTime = billingViewModel.WSDLGenerationTime,
                WSDLSign = billingViewModel.WSDLSign,
                WSDLToken = billingViewModel.WSDLToken,
                UseHomologacion = billingViewModel.UseHomologacion,
            };
        }

        #endregion

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
                //SaleConditionId = boxViewModel.SaleConditionId,
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
                //SaleConditionId = bankParameterViewModel.SaleConditionId,
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
        public static SaleViewModel? ToSaleViewModel(Sale sale)
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
                TotalIVA27 = sale.TotalIVA27
                //SaleConditionId = sale.SaleConditionId,
                //Clients = clients,
                //SaleConditions = saleConditions,
                //PriceLists = priceLists,
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
                //SaleConditionId = saleViewModel.SaleConditionId,
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
