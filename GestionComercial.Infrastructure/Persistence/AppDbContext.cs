using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Entities.Budget;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Entities.Masters.Security;
using GestionComercial.Domain.Entities.Purchases;
using GestionComercial.Domain.Entities.Sales;
using GestionComercial.Domain.Entities.Stock;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GestionComercial.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        /* --------Agregar DbSets para cada entidad --------- */

        //ACCOUNTING BOOK
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountingSeat> AccountingSeats { get; set; }
        public DbSet<AccountingSeatDetail> AccountingSeatDetails { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<AccountVinculation> AccountVinculations { get; set; }


        //AFIP
        public DbSet<CbteType> CbteTypes { get; set; }
        public DbSet<Concept> Concepts { get; set; }
        public DbSet<IvaCondition> IvaConditions { get; set; }
        public DbSet<Optional> Optionals { get; set; }
        public DbSet<Tax> Taxes { get; set; }
        public DbSet<Tribute> Tributes { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }


        //BOX AND BANK
        public DbSet<Acreditation> Acreditations { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<BankParameter> BankParameters { get; set; }
        public DbSet<Box> Boxes { get; set; }
        public DbSet<Debitation> Debitations { get; set; }


        //BUDGET
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BudgetDetail> BudgetDetails { get; set; }
        public DbSet<BudgetDetailTmp> BudgetDetailTmps { get; set; }


        //MASTER/CONFIGURATION
        public DbSet<EmailParameter> EmailParameters { get; set; }
        public DbSet<GeneralParameter> GeneralParameters { get; set; }
        public DbSet<PcParameter> PcParameters { get; set; }
        public DbSet<PrinterParameter> PrinterParameters { get; set; }


        //MASTER/SECURITY
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }


        //MASTER
        public DbSet<Billing> Billings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CommerceData> CommerceDatas { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<SaleCondition> SaleConditions { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }


        //PURCHASE
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<PurchaseDetailTmp> PurchaseDetailTmps { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        public DbSet<PurchaseOrderDetailTmp> PurchaseOrderDetailTmps { get; set; }
        public DbSet<PurchasePayMetodDetail> PurchasePayMetodDetails { get; set; }


        //SALE
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<ReservationDetail> ReservationDetails { get; set; }
        public DbSet<ReservationDetailTmp> ReservationDetailTmps { get; set; }
        public DbSet<ReservationPayMetodDetail> ReservationPayMetodDetails { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<SaleDetailTmp> SaleDetailTmps { get; set; }
        public DbSet<SalePayMetodDetail> SalePayMetodDetails { get; set; }


        //STOCK
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 📌 Seed de roles iniciales (opcional)
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Developer", NormalizedName = "Developer".ToUpper() },
                new IdentityRole { Id = "2", Name = "Administrador", NormalizedName = "Administrador".ToUpper() },
                new IdentityRole { Id = "3", Name = "Supervisor", NormalizedName = "Supervisor".ToUpper() },
                new IdentityRole { Id = "4", Name = "Operador", NormalizedName = "Operador".ToUpper() },
                new IdentityRole { Id = "5", Name = "Cajero", NormalizedName = "Cajero".ToUpper() }
            );

            // Configuración de la relación de RolePermission
            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany() // IdentityRole no tiene propiedad de navegación por defecto
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configuración de la relación de UserPermission
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany() // IdentityUser no tiene propiedad de navegación por defecto
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);


            //PRECICION DE DECIMALES
            modelBuilder.Entity<Tax>().Property(x => x.Rate).HasPrecision(18, 2);
            modelBuilder.Entity<Article>().Property(x => x.Cost).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.MinimalStock).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.RealCost).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.Replacement).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.Stock).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.Umbral).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.Utility).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.Bonification).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.InternalTax).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.CostWithTaxes).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.SalePrice).HasPrecision(18, 4);
            modelBuilder.Entity<Article>().Property(x => x.SalePriceWithTaxes).HasPrecision(18, 4);
            modelBuilder.Entity<Client>().Property(x => x.Sold).HasPrecision(18, 2);
            modelBuilder.Entity<Client>().Property(x => x.CreditLimit).HasPrecision(18, 2);
            modelBuilder.Entity<PriceList>().Property(x => x.Utility).HasPrecision(18, 4);
            modelBuilder.Entity<Provider>().Property(x => x.Sold).HasPrecision(18, 4);



            //INDEX
            modelBuilder.Entity<Account>()
                .HasIndex(a => new
                {
                    //a.AccountGroupNumber,
                    a.AccountTypeId,
                    a.AccountSubGroupNumber1,
                    a.AccountSubGroupNumber2,
                    a.AccountSubGroupNumber3,
                    a.AccountSubGroupNumber4,
                    a.AccountSubGroupNumber5
                })
                .IsUnique()
                .HasDatabaseName("Account_AccountNumber_Index");

            modelBuilder.Entity<AccountingSeat>()
                .HasIndex(a => new

                {
                    a.AccountingSeatNumber,
                    a.AccountingSeatYear
                })
                .IsUnique()
                .HasDatabaseName("Accounting_SeatNumber_Index");

            modelBuilder.Entity<AccountVinculation>()
               .HasIndex(av => new

               {
                   av.AccountId,
                   av.VinculatedAccountId
               })
               .IsUnique()
               .HasDatabaseName("AccountVinculation_AccountId_ViculatedAccountId_Index");

            //modelBuilder.Entity<BankParameter>()
            //  .HasIndex(bp => new

            //  {
            //      bp.BankId,
            //      bp.SaleConditionId,
            //  })
            //  .IsUnique()
            //  .HasDatabaseName("Bank_SaleCondition_Index");


            modelBuilder.Entity<Purchase>()
            .HasIndex(p => new { p.ProviderId, p.BillNumber, p.BillPoint })
            .IsUnique()
            .HasDatabaseName("BillNumber_Provider_Index");

            modelBuilder.Entity<CommerceData>()
                .HasIndex(cd => new { cd.CUIT })
                .IsUnique()
                .HasDatabaseName("CommerceData_Cuit_Index");

            modelBuilder.Entity<Tax>()
               .HasIndex(t => new { t.Description })
               .IsUnique()
               .HasDatabaseName("Tax_Description_Index");

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.UserName })
                .IsUnique()
                .HasDatabaseName("User_UserName_Index");

            modelBuilder.Entity<User>()
                .HasIndex(u => new { u.Email })
                .IsUnique()
                .HasDatabaseName("User_Email_Index");

            modelBuilder.Entity<Client>()
                .HasIndex(c => new { c.OptionalCode })
                .IsUnique()
                .HasDatabaseName("Client_Code_Index");

            modelBuilder.Entity<Client>()
                .HasIndex(c => new { c.DocumentNumber, c.DocumentTypeId })
                .IsUnique()
                .HasDatabaseName("Client_Document_Index");

            modelBuilder.Entity<PriceList>()
                .HasIndex(c => new { c.Description })
                .IsUnique()
                .HasDatabaseName("PriceList_Description_Index");

            modelBuilder.Entity<Provider>()
                .HasIndex(c => new { c.OptionalCode })
                .IsUnique()
                .HasDatabaseName("Provider_Code_Index");

            modelBuilder.Entity<Provider>()
                .HasIndex(c => new { c.DocumentNumber, c.DocumentTypeId })
                .IsUnique()
                .HasDatabaseName("Provider_Document_Index");

            modelBuilder.Entity<Article>()
                .HasIndex(p => new { p.Code })
                .IsUnique()
                .HasDatabaseName("Product_Code_Index");

            modelBuilder.Entity<Article>()
               .HasIndex(p => new { p.BarCode })
               .IsUnique()
               .HasDatabaseName("Product_BarCode_Index")
               .HasFilter("[BarCode] IS NOT NULL AND [BarCode] <> ''");

            modelBuilder.Entity<State>()
              .HasIndex(s => new { s.Name })
              .IsUnique()
              .HasDatabaseName("State_Name_Index");

            modelBuilder.Entity<Sale>()
              .HasIndex(s => new { s.SalePoint, s.SaleNumber })
              .IsUnique()
              .HasDatabaseName("Sale_SalePoint-Number_Index");

            modelBuilder.Entity<Invoice>()
              .HasIndex(s => new { s.SaleId, s.CompTypeId })
              .IsUnique()
              .HasDatabaseName("Invoice_SaleId-CompTypeId_Index");

            modelBuilder.Entity<City>()
              .HasIndex(c => new { c.Name, c.StateId, c.AfipId })
              .IsUnique()
              .HasDatabaseName("State_Name_Index");

            modelBuilder.Entity<Permission>()
             .HasIndex(c => new { c.Name, c.ModuleType })
             .IsUnique()
             .HasDatabaseName("Permision_Name_Index");

            modelBuilder.Entity<UserPermission>()
             .HasIndex(up => new { up.UserId, up.PermissionId, up.IsDeleted, up.IsEnabled })
             .IsUnique()
             .HasDatabaseName("Permision_Name_Index");


        }


    }
}
