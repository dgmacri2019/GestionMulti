using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.PriceLists;
using GestionComercial.Domain.DTOs.Provider;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionComercial.Desktop.ViewModels.PriceList
{
    internal class PriceListListViewModel
    {
        private readonly PriceListsApiService _pricelistsApiService;

        public ObservableCollection<PriceListViewModel> PriceLists { get; set; } = [];



        public PriceListListViewModel(bool isEnabled, bool isDeleted)
        {
            _pricelistsApiService = new PriceListsApiService();
            GetAllAsync(isEnabled, isDeleted);
        }

        public PriceListListViewModel(string description, bool isEnabled, bool isDeleted)
        {
            _pricelistsApiService = new PriceListsApiService();
            SearchAsync(description, isEnabled, isDeleted);
        }

        private async Task SearchAsync(string description, bool isEnabled, bool isDeleted)
        {
            try
            {
                List<PriceListViewModel> priceLists = await _pricelistsApiService.SearchAsync(description, isEnabled, isDeleted);
                PriceLists.Clear();
                foreach (var p in priceLists)
                {
                    PriceLists.Add(p);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private async Task<ObservableCollection<PriceListViewModel>> GetAllAsync(bool isEnabled, bool isDeleted)
        {
            List<PriceListViewModel> priceLists = await _pricelistsApiService.GetAllAsync(isEnabled, isDeleted);
            PriceLists.Clear();
            foreach (var p in priceLists)
            {
                PriceLists.Add(p);
            }

            return PriceLists;
        }
    }
}

