using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Banks
{
    /// <summary>
    /// Lógica de interacción para EditBankParameterControlView.xaml
    /// </summary>
    public partial class EditBankParameterControlView : UserControl
    {
        private readonly BanksApiService _bankApiService;

        private readonly int BankParameterId;
        private BankParameterViewModel BankParameterViewModel { get; set; }

        public event Action ParametroActualizado;

        public EditBankParameterControlView(int bankParameterId)
        {
            InitializeComponent();
            BankParameterId = bankParameterId;
            _bankApiService = new BanksApiService();
            FindBankParameterAsync();
            if (BankParameterId > 0)
            {
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
            }
            else
            {
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
        }



        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblError.MaxWidth = this.ActualWidth;
        }

        private async void FindBankParameterAsync()
        {
            BankAndBoxResponse result = await _bankApiService.GetBankParameterByIdAsync(BankParameterId, true, false);
            if (result.Success)
            {
                BankParameterViewModel = result.BankParameterViewModel;
                if (BankParameterId == 0)
                {
                    BankParameterViewModel.CreateUser = App.UserName;
                }
                DataContext = BankParameterViewModel;
            }
            else
                lblError.Text = result.Message;
        }


        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                btnAdd.IsEnabled = false;
                lblError.Text = string.Empty;
                BankParameterViewModel.CreateUser = App.UserName;

                BankParameter bankParameter = ConverterHelper.ToBankParameter(BankParameterViewModel, BankParameterViewModel.Id == 0);
                GeneralResponse resultUpdate = await _bankApiService.AddBankParameterAsync(bankParameter);
                if (resultUpdate.Success)
                    ParametroActualizado?.Invoke(); // para notificar a la vista principal
                else
                    lblError.Text = resultUpdate.Message;
                btnAdd.IsEnabled = true;


            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;

                btnUpdate.IsEnabled = false;
                lblError.Text = string.Empty;
                BankParameterViewModel.UpdateUser = App.UserName;
                BankParameterViewModel.UpdateDate = DateTime.Now;

                BankParameter bankParameter = ConverterHelper.ToBankParameter(BankParameterViewModel, BankParameterViewModel.Id == 0);
                GeneralResponse resultUpdate = await _bankApiService.UpdateBankParameterAsync(bankParameter);
                if (resultUpdate.Success)
                    ParametroActualizado?.Invoke(); // para notificar a la vista principal
                else
                    lblError.Text = resultUpdate.Message;
                btnUpdate.IsEnabled = true;

            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ParametroActualizado?.Invoke(); // para notificar a la vista principal
        }

        private void txtRate_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == ".")
                result = false;
            else
                result = true;
            e.Handled = result;
        }

        private void txtAcreditation_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado))
                result = false;
            else
                result = true;
            e.Handled = result;
        }

        private void txtAcreditation_LostFocus(object sender, RoutedEventArgs e)
        {
            txtAcreditation.Text = txtAcreditation.Text.Replace(".", ",");
        }

        private void txtRate_LostFocus(object sender, RoutedEventArgs e)
        {
            txtRate.Text = txtRate.Text.Replace(".", ",");
        }
    }
}
