using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Masters.Security;
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

        //AFIP
        public DbSet<Tax> Taxes { get; set; }


        //MASTER
        public DbSet<Billing> Billings { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CommerceData> CommerceDatas { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<State> States { get; set; }

        //MASTER/SECURITY
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }

        //STOCK
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 📌 Seed de roles iniciales (opcional)
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Developer", NormalizedName = "DEVELOPER" },
                new IdentityRole { Id = "2", Name = "Administrator", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "3", Name = "Supervisor", NormalizedName = "SUPERVISOR" },
                new IdentityRole { Id = "4", Name = "Operator", NormalizedName = "OPERADOR" },
                new IdentityRole { Id = "5", Name = "Cashier", NormalizedName = "CAJERO" }
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
            modelBuilder.Entity<Client>().Property(x => x.Sold).HasPrecision(18, 4);
            modelBuilder.Entity<PriceList>().Property(x => x.Utility).HasPrecision(18, 4);
            modelBuilder.Entity<Provider>().Property(x => x.Sold).HasPrecision(18, 4);



            //INDEX
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

            modelBuilder.Entity<City>()
              .HasIndex(c => new { c.Name, c.StateId, c.AfipId })
              .IsUnique()
              .HasDatabaseName("State_Name_Index");

            modelBuilder.Entity<Permission>()
             .HasIndex(c => new { c.Name })
             .IsUnique()
             .HasDatabaseName("Permision_Name_Index");


        }


    }
}
