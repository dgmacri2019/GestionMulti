using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestionComercial.Desktop.Controls.Banks
{
    /// <summary>
    /// Lógica de interacción para EditBankControlView.xaml
    /// </summary>
    public partial class EditBankControlView : UserControl
    {
        private readonly BanksApiService _bankApiService;
        private readonly int BankId;
        private BankViewModel BankViewModel { get; set; }

        public event Action BancoActualizado;

        public EditBankControlView(int bankId)
        {
            InitializeComponent();
            _bankApiService = new BanksApiService();
            BankId = bankId;
            FindBank();
            if (BankId > 0)
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

        private async void FindBank()
        {
            BankAndBoxResponse result = await _bankApiService.GetBankByIdAsync(BankId, true, false);
            if (result.Success)
            {
                BankViewModel = result.BankViewModel;
                if (BankId == 0)
                {
                    BankViewModel.CreateUser = App.UserName;
                }
                DataContext = BankViewModel;
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
                BankViewModel.CreateUser = App.UserName;
                BankViewModel.BankName = BankViewModel.BankName.ToUpper();
                Bank bank = ConverterHelper.ToBank(BankViewModel, BankViewModel.Id == 0);
                GeneralResponse resultUpdate = await _bankApiService.AddBankAsync(bank);
                if (resultUpdate.Success)
                    BancoActualizado?.Invoke(); // para notificar a la vista principal
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
                BankViewModel.BankName = BankViewModel.BankName.ToUpper();
                BankViewModel.UpdateUser = App.UserName;
                BankViewModel.UpdateDate = DateTime.Now;

                Bank bank = ConverterHelper.ToBank(BankViewModel, BankViewModel.Id == 0);
                GeneralResponse resultUpdate = await _bankApiService.UpdateBankAsync(bank);
                if (resultUpdate.Success)
                    BancoActualizado?.Invoke(); // para notificar a la vista principal
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
            BancoActualizado?.Invoke(); // para notificar a la vista principal
        }

        private void txtFromCredit_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtFromCredit_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFromCredit.Text = txtFromCredit.Text.Replace(".", ",");
        }

        private void txtFromCredit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == "," || textoIngresado == "-")
                result = false;
            else
                result = true;
            e.Handled = result;
        }

        private void txtFromCredit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtSold_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtSold_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == "," || textoIngresado == "-")
                result = false;
            else
                result = true;
            e.Handled = result;
        }

        private void txtSold_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSold.Text = txtSold.Text.Replace(".", ",");
        }

        private void txtSold_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtFromDebit_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtFromDebit_LostFocus(object sender, RoutedEventArgs e)
        {
            txtFromDebit.Text = txtFromDebit.Text.Replace(".", ",");
        }

        private void txtFromDebit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string textoIngresado = e.Text;
            bool result = false;
            if (ValidatorHelper.IsNumeric(textoIngresado) || textoIngresado == "." || textoIngresado == "," || textoIngresado == "-")
                result = false;
            else
                result = true;
            e.Handled = result;
        }

        private void txtFromDebit_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
