using GestionComercial.Domain.Entities.Stock;

namespace GestionComercial.Domain.Response
{
    public class CategoryResponse : GeneralResponse
    {
        public List<Category> Categories { get; set; }
        public Category? Category { get; set; }
    }
}
