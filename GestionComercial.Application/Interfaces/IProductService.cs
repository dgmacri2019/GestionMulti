using GestionComercial.Domain.DTOs;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;

namespace GestionComercial.Applications.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task<GeneralResponse> AddAsync(Product product);
        Task<GeneralResponse> UpdateAsync(Product product);
        Task<GeneralResponse> DeleteAsync(int id);
        Task<IEnumerable<ProductWithPricesDto>> GetProductsWithPricesAsync();


        IEnumerable<Product> GetAll();
        Product GetById(int id);
        GeneralResponse Add(Product product);
        GeneralResponse Update(Product product);
        GeneralResponse Delete(int id);
    }
}
