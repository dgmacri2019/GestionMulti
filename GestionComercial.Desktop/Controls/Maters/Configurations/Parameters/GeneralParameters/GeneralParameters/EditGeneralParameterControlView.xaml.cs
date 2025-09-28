using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.GeneralParameters.GeneralParameters
{
    /// <summary>
    /// Lógica de interacción para EditGeneralParameterControlView.xaml
    /// </summary>
    public partial class EditGeneralParameterControlView : UserControl
    {
        private readonly ParametersApiService _parametersApiService;
        private GeneralParameter? GeneralParameter { get; set; }

        public event Action ParametroGeneralActualizado;
        public EditGeneralParameterControlView()
        {
            InitializeComponent();
            ParametroGeneralActualizado?.Invoke();
            _parametersApiService = new ParametersApiService();
            GeneralParameter = ParameterCache.Instance.GetGeneralParameter();
            if (GeneralParameter == null)
                GeneralParameter = new GeneralParameter
                {
                    CreateDate = DateTime.Now,
                    CreateUser = LoginUserCache.UserName,
                    IsDeleted = false,
                    IsEnabled = true,
                    ProductBarCodeWeight = true,
                    UsePostMethod = false,
                    SumQuantityItems = true,
                };
            else
            {
                GeneralParameter.UpdateDate = DateTime.Now;
                GeneralParameter.UpdateUser = LoginUserCache.UserName;

            }

            DataContext = GeneralParameter;
        }

        private async void BtnUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                lblConfirm.Text = string.Empty;
                btnUpdate.IsEnabled = false;


                GeneralResponse resultUpdate = await _parametersApiService.UpdateGeneralParameterAsync(GeneralParameter);
                if (resultUpdate.Success)
                {
                    lblConfirm.Text = resultUpdate.Message;
                    ParametroGeneralActualizado?.Invoke();
                }
                else
                    msgError(resultUpdate.Message);
                btnUpdate.IsEnabled = true;

            }
            catch (Exception ex)
            {
                msgError(ex.Message);
            }
        }

        private void BtnCancel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ParametroGeneralActualizado?.Invoke();
        }


        private void msgError(string msg)
        {
            lblError.Text = msg;
        }
    }
}
