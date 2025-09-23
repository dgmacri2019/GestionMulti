using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PurchaseAndSales
{
    /// <summary>
    /// Lógica de interacción para EditPcParameterControlView.xaml
    /// </summary>
    public partial class EditPcParameterControlView : UserControl
    {

        private readonly ParametersApiService _parametersApiService;
        private readonly int ParameterId;
        private PcParameter PcParameter { get; set; }

        public event Action PuntoVentaActualizado;

        public EditPcParameterControlView(int id)
        {
            InitializeComponent();
            _parametersApiService = new ParametersApiService();
            ParameterId = id;

            _ = FindParameterAsync();

        }

        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GridGeneral.MaxWidth = this.ActualWidth;
            lblError.MaxWidth = this.ActualWidth;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            PuntoVentaActualizado?.Invoke();
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                btnUpdate.IsEnabled = false;
                PcParameter.UpdateUser = LoginUserCache.UserName;
                PcParameter.UpdateDate = DateTime.Now;

                GeneralResponse resultUpdate = await _parametersApiService.UpdatePcParameterAsync(PcParameter);
                if (resultUpdate.Success)
                    PuntoVentaActualizado?.Invoke();
                else
                    msgError(resultUpdate.Message);
                btnUpdate.IsEnabled = true;

            }
            catch (Exception ex)
            {
                msgError(ex.Message);
            }
        }


        private async Task FindParameterAsync()
        {
            PcParameterResponse result = await _parametersApiService.GetPcParameterByIdAsync(ParameterId);

            if (result.Success)
            {
                PcParameter = result.PcParameter;
                if (ParameterId == 0)
                {
                    PcParameter.CreateUser = LoginUserCache.UserName;
                }

                DataContext = PcParameter;
            }
            else
                msgError(result.Message);
        }

        private void msgError(string msg)
        {
            lblError.Text = msg;
        }


    }
}
