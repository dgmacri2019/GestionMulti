using GestionComercial.Applications.Interfaces;
using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Masters;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using GestionComercial.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace GestionComercial.Applications.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly DBHelper _dBHelper;


        public ProductService(AppDbContext context)
        {
            _context = context;
            _dBHelper = new DBHelper();
        }

        public GeneralResponse Add(Product product)
        {
            _context.Products.Add(product);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> AddAsync(Product product)
        {
            _context.Products.Add(product);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public GeneralResponse Delete(int id)
        {
            Product product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                return _dBHelper.SaveChanges(_context);
            }
            return new GeneralResponse { Success = false, Message = "Producto no encontrado" };
        }

        public async Task<GeneralResponse> DeleteAsync(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            return new GeneralResponse { Success = false, Message = "Producto no encontrado" };
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public Product GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public GeneralResponse Update(Product product)
        {
            _context.Products.Update(product);
            return _dBHelper.SaveChanges(_context);
        }

        public async Task<GeneralResponse> UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            return await _dBHelper.SaveChangesAsync(_context);
        }

        public async Task<IEnumerable<ProductWithPricesDto>> GetProductsWithPricesAsync()
        {
            // Incluimos las listas de precios; asegúrate de que la propiedad esté activa en Product
            ICollection<PriceList> priceLists = _context.PriceLists.Where(pl =>pl.IsEnabled && !pl.IsDeleted).ToList();
            List<Product> products = await _context.Products
                //.Include(p => p.PriceLists)
                .ToListAsync();


            IEnumerable<ProductWithPricesDto> result = products.Select(p => new ProductWithPricesDto
            {
                Id = p.Id,
                Code = p.Code,
                Description = p.Description,
                Cost = p.Cost,
                BarCode = p.BarCode,
                PriceLists = priceLists.Select(pl => new PriceListItemDto
                {
                    Description = pl.Description,
                    Utility = pl.Utility,
                    FinalPrice = p.Cost + (p.Cost * pl.Utility / 100)
                })/*.OrderBy(pl => pl.Utility)*/.ToList() // Ordenamos para que la lista 1 (utility=0) aparezca primero, luego las que ofrecen descuentos
            });

            return result;
        }
    }
}
