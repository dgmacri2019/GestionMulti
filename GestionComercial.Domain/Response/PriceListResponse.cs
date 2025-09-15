using GestionComercial.Domain.DTOs.PriceLists;

namespace GestionComercial.Domain.Response
{
    public class PriceListResponse : GeneralResponse
    {
        public string Description { get; set; }

        public PriceListViewModel PriceListViewModel { get; set; }
        public List<PriceListViewModel> PriceListViewModels { get; set; }

    }
}
