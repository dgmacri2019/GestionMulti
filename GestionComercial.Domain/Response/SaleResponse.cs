using GestionComercial.Domain.DTOs.Sale;

namespace GestionComercial.Domain.Response
{
    public class SaleResponse : GeneralResponse
    {

        public SaleViewModel SaleViewModel { get; set; }
        public List<SaleViewModel> SaleViewModels { get; set; }

        public int LastSaleNumber { get; set; }
        public int SaleId { get; set; }

        public byte[]? Bytes { get; set; }
    }
}
