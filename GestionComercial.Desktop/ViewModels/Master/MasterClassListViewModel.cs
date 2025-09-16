using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.Services.Hub;
using GestionComercial.Desktop.Utils;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Response;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using static GestionComercial.Domain.Constant.Enumeration;
using static GestionComercial.Domain.Notifications.MasterClassChangeNotification;

namespace GestionComercial.Desktop.ViewModels.Master
{
    internal class MasterClassListViewModel : BaseViewModel
    {
        private readonly MasterClassApiService _masterClassApiService;
        private readonly MasterClassHubService _hubService;



        // 🔹 Lista observable para bindear al DataGrid
        public ObservableCollection<Category> Categories { get; } = [];

        // 🔹 Propiedades de filtros
        private string _nameFilter = string.Empty;
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                if (_nameFilter != value)
                {
                    _nameFilter = value;
                    OnPropertyChanged();
                    _ = LoadMastersAsync(); // 🔹 ejecuta búsqueda al escribir
                }
            }
        }

        private bool _isEnabledFilter = true;
        public bool IsEnabledFilter
        {
            get => _isEnabledFilter;
            set
            {
                if (_isEnabledFilter != value)
                {
                    _isEnabledFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isDeletedFilter = false;
        public bool IsDeletedFilter
        {
            get => _isDeletedFilter;
            set
            {
                if (_isDeletedFilter != value)
                {
                    _isDeletedFilter = value;
                    OnPropertyChanged();
                }
            }
        }

        // 🔹 Command para buscar
        public ICommand SearchCommand { get; }
        public ICommand ToggleEnabledCommand { get; }


        public string ToggleEnabledText => IsEnabledFilter ? "Ver Inhabilitados" : "Ver Habilitados";





        public MasterClassListViewModel()
        {
            _masterClassApiService = new MasterClassApiService();

            var hubUrl = string.Format("{0}hubs/masterclass", App.Configuration["ApiSettings:BaseUrl"]);


            _hubService = new MasterClassHubService(hubUrl);
            _hubService.ClaseMaestraCambiado += OnClaseMaestraCambiado;
            ToggleEnabledCommand = new RelayCommand1(async _ => await ToggleEnabled());
            SearchCommand = new RelayCommand1(async _ => await LoadMastersAsync());

            _ = _hubService.StartAsync();
            _ = LoadMastersAsync(); // carga inicial
        }

        private async Task ToggleEnabled()
        {
            IsEnabledFilter = !IsEnabledFilter;
            OnPropertyChanged(nameof(ToggleEnabledText));
            await LoadMastersAsync();
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
                    MasterCache.Instance.SetData(result.States, result.SaleConditions, result.IvaConditions,
                        result.DocumentTypes, result.Measures, result.Taxes, result.CommerceData);
                }
                else
                    MessageBox.Show($"Error al cargar clientes, el error fue:\n{result.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

                MasterCache.Reading = false;
            }


        }

        // 🔹 SignalR recibe notificación y actualiza cache + lista
        private async void OnClaseMaestraCambiado(ClaseMaestraChangeNotification notification)
        {
            switch (notification.ChangeClass)
            {
                case ChangeClass.PriceList:
                    break;
                case ChangeClass.CommerceData:
                    break;
                default:
                    {
                        MasterClassResponse result = await _masterClassApiService.GetAllAsync();
                        if (result.Success)
                            await App.Current.Dispatcher.InvokeAsync(() =>
                            {
                                MasterCache.Instance.ClearCache();
                                MasterCache.Instance.SetData(result.States, result.SaleConditions, result.IvaConditions,
                                    result.DocumentTypes, result.Measures, result.Taxes, result.CommerceData);

                                _ = LoadMastersAsync();
                            });
                        else
                            MsgBoxAlertHelper.MsgAlertError($"Error al cargar clase maestra, el error fue:\n{result.Message}");
                        break;
                    }
            }
        }
    }
}
