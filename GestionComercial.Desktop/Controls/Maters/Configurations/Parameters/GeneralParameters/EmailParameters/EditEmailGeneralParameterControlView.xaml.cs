using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters.Configuration;
using GestionComercial.Domain.Response;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.GeneralParameters.EmailParameters
{
    /// <summary>
    /// Lógica de interacción para EditEmailGeneralParameterControlView.xaml
    /// </summary>
    public partial class EditEmailGeneralParameterControlView : UserControl
    {
        private readonly ParametersApiService _parametersApiService;
        private EmailParameter? EmailParameter { get; set; }

        public event Action ParametroEmailActualizado;
        public EditEmailGeneralParameterControlView()
        {
            InitializeComponent();
            ParametroEmailActualizado?.Invoke();
            _parametersApiService = new ParametersApiService();
            EmailParameter = ParameterCache.Instance.GetEmailParameter();
            if (EmailParameter == null)
                EmailParameter = new EmailParameter
                {
                    CreateDate = DateTime.Now,
                    CreateUser = LoginUserCache.UserName,
                    IsDeleted = false,
                    IsEnabled = true,
                };
            else
            {
                EmailParameter.UpdateDate = DateTime.Now;
                EmailParameter.UpdateUser = LoginUserCache.UserName;

            }
            DataContext = EmailParameter;
        }



        private async void BtnUpdate_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                lblConfirm.Text = string.Empty;
                btnUpdate.IsEnabled = false;


                GeneralResponse resultUpdate = await _parametersApiService.UpdateEmailParameterAsync(EmailParameter);
                if (resultUpdate.Success)
                {
                    lblConfirm.Text = resultUpdate.Message;
                    ParametroEmailActualizado?.Invoke();
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
            ParametroEmailActualizado?.Invoke();
        }




        private void msgError(string msg)
        {
            lblError.Text = msg;
        }
    }
}
