using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Response;
using System.Windows;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Master
{
    internal class MasterClassListViewModel : BaseViewModel
    {
        private readonly MasterClassApiService _masterClassApiService;
        private readonly MasterClassHubService _hubService;
        public MasterClassListViewModel()
        {
            _masterClassApiService = new MasterClassApiService();

            var hubUrl = string.Format("{0}hubs/masterclass", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new MasterClassHubService(hubUrl);
            _hubService.ClaseMaestraCambiado += OnClaseMaestraCambiado;


            _ = _hubService.StartAsync();
            _ = LoadMastersAsync(); // carga inicial
        }


        // 🔹 Carga clientes aplicando filtros
        public async Task LoadMastersAsync()
        {
            if (!MasterCache.Instance.HasData)
            {
                MasterCache.Reading = true;
                MasterClassResponse result = await _masterClassApiService.GetAllAsync();
                if (result.Success)
                {
                    MasterCache.Instance.SetData(result.PriceLists, result.States,
                        result.SaleConditions, result.IvaConditions, result.DocumentTypes, result.Categories, result.Measures, result.Taxes);
                }
                else
                    MessageBox.Show($"Error al cargar clientes, el error fue:\n{result.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

                MasterCache.Reading = false;
            }


        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnClaseMaestraCambiado(ClaseMaestraChangeNotification notification)
        {
            MasterClassResponse result = await _masterClassApiService.GetAllAsync();
            if (result.Success)
                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                    MasterCache.Instance.ClearCache();
                    MasterCache.Instance.SetData(result.PriceLists, result.States, result.SaleConditions, result.IvaConditions,
                        result.DocumentTypes, result.Categories, result.Measures, result.Taxes);

                    _ = LoadMastersAsync();
                });
            else
                MessageBox.Show($"Error al cargar clase maestra, el error fue:\n{result.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
