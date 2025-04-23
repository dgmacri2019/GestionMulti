namespace GestionComercial.Domain.DTOs.Stock
{
    public class ArticleFilterDto
    {
        public string Description { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public int Percentage { get; set; } = 0;
        public int CategoryId { get; set; } = 0;
        public IProgress<int> Progress { get; set; } = new Progress<int>();
        public int Id { get; set; } = 0;
        public string BarCode { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
