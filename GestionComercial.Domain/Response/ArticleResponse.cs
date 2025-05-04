using GestionComercial.Domain.DTOs.Stock;

namespace GestionComercial.Domain.Response
{
    public class ArticleResponse : GeneralResponse
    {
        public string BarCode { get; set; } = string.Empty;

        public ArticleViewModel ArticleViewModel { get; set; }
    }
}
