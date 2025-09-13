using GestionComercial.Domain.DTOs.Stock;

namespace GestionComercial.Domain.Response
{
    public class CategoryResponse : GeneralResponse
    {
        public List<CategoryViewModel> Categories { get; set; }
        public CategoryViewModel? Category { get; set; }
    }
}
