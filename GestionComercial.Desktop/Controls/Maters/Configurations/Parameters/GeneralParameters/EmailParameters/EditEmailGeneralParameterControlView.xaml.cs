using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.Entities.Masters.Configuration;
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
    }
}
