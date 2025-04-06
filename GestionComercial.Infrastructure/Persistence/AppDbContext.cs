using GestionComercial.Domain.Entities.Afip;
using GestionComercial.Domain.Entities.Masters;
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
        public DbSet<City> Cities { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<PriceList> PriceLists { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<State> States { get; set; }

        //MASTER/SECURITY


        //STOCK
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 📌 Seed de roles iniciales (opcional)
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Developer", NormalizedName = "DEVELOPER" },
                new IdentityRole { Id = "2", Name = "Administrator", NormalizedName = "ADMINISTRADOR" },
                new IdentityRole { Id = "3", Name = "Supervisor", NormalizedName = "SUPERVISOR" },
                new IdentityRole { Id = "4", Name = "Operator", NormalizedName = "OPERADOR" }
            );




            //PRECICION DE DECIMALES
            modelBuilder.Entity<Tax>().Property(x => x.Rate).HasPrecision(18,2);
            modelBuilder.Entity<Product>().Property(x => x.Cost).HasPrecision(18, 4);
            modelBuilder.Entity<Product>().Property(x => x.MinimalStock).HasPrecision(18, 4);
            modelBuilder.Entity<Product>().Property(x => x.RealCost).HasPrecision(18, 4);
            modelBuilder.Entity<Product>().Property(x => x.Replacement).HasPrecision(18, 4);
            modelBuilder.Entity<Product>().Property(x => x.Stock).HasPrecision(18, 4);
            modelBuilder.Entity<Product>().Property(x => x.Umbral).HasPrecision(18, 4);
            modelBuilder.Entity<Client>().Property(x => x.Sold).HasPrecision(18, 4);
            modelBuilder.Entity<PriceList>().Property(x => x.Utility).HasPrecision(18, 4);
            modelBuilder.Entity<Provider>().Property(x => x.Sold).HasPrecision(18, 4);



            //INDEX
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

            modelBuilder.Entity<Product>()
                .HasIndex(p => new { p.Code })
                .IsUnique()
                .HasDatabaseName("Product_Code_Index");

            modelBuilder.Entity<Product>()
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


        }


    }
}
