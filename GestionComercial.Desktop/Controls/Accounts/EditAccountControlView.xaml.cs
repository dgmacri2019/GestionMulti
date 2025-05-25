using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.DTOs.Stock;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Entities.Stock;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Accounts
{
    /// <summary>
    /// Lógica de interacción para EditAccountControlView.xaml
    /// </summary>
    public partial class EditAccountControlView : UserControl
    {
        private readonly AccountsApiService _accountsApiService;
        private readonly int AccountId;
        private List<Account> Accounts { get; set; }
        private List<AccountType> AccountTypes { get; set; }
        private AccountViewModel accountViewModel { get; set; }


        public event Action CuentaActualizada;

        public EditAccountControlView(int accountId)
        {
            InitializeComponent();
            _accountsApiService = new AccountsApiService();
            AccountId = accountId;
            if (AccountId > 0)
            {
                FindAccountAsync();
                btnAdd.Visibility = Visibility.Hidden;
                btnUpdate.Visibility = Visibility.Visible;
                txtAccountGroup5.Visibility = Visibility.Hidden;
                cbAccountGroup5.Visibility = Visibility.Visible;
            }
            else
            {
                GetAllAccountsAsync();
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
        }

        private async void FindAccountAsync()
        {
            AccountResponse result = await _accountsApiService.GetByIdAsync(AccountId);
            if (result.Success)
            {
                accountViewModel = result.AccountViewModel;
                if (AccountId == 0)
                {
                    accountViewModel.CreateUser = App.UserName;
                }
                DataContext = accountViewModel;
            }
            else
                lblError.Text = result.Message;
        }

        private async void GetAllAccountsAsync()
        {
            AccountTypes = await _accountsApiService.GetAllAccountTypesAsync(true, false, false);
            cbAccountType.ItemsSource = AccountTypes;
            cbAccountType.SelectedValue = "0";
            Accounts = await _accountsApiService.GetAllAccountAsync(true, false, true);
        }

        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblError.MaxWidth = this.ActualWidth;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CuentaActualizada?.Invoke(); // para notificar a la vista principal
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnUpdate.IsEnabled = false;
                lblError.Text = string.Empty;
                accountViewModel.UpdateUser = App.UserName;
                accountViewModel.UpdateDate = DateTime.Now;

                Account account = ConverterHelper.ToAccount(accountViewModel, accountViewModel.Id == 0);
                GeneralResponse resultUpdate = await _accountsApiService.UpdateAsync(account);
                if (resultUpdate.Success)
                    CuentaActualizada?.Invoke(); // para notificar a la vista principal
                else
                    lblError.Text = resultUpdate.Message;
                btnUpdate.IsEnabled = true;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            btnAdd.IsEnabled = false;
            lblError.Text = string.Empty;
            Account account = ConverterHelper.ToAccount(accountViewModel, accountViewModel.Id == 0);
            GeneralResponse resultUpdate = await _accountsApiService.AddAsync(account);
            if (resultUpdate.Success)
                CuentaActualizada?.Invoke(); // para notificar a la vista principal
            else
                lblError.Text = resultUpdate.Message;
            btnAdd.IsEnabled = true;
        }

        private void cbAccountType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAccountType.SelectedItem is AccountType tipo)
            {
                if (tipo.Id == 0)
                    return;
                List<Account> sub1 =
                [
                    new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 1",
                    },
                    .. Accounts
                     .Where(a => a.AccountTypeId == tipo.Id
                            && a.AccountSubGroupNumber2 == 0
                            && a.AccountSubGroupNumber2 == 0
                            && a.AccountSubGroupNumber3 == 0
                            && a.AccountSubGroupNumber4 == 0
                            && a.AccountSubGroupNumber5 == 0)
                     .GroupBy(c => c.AccountSubGroupNumber1)
                     .Select(g => g.First())
                     .OrderBy(c => c.AccountSubGroupNumber1)
                     .ToList(),
                ];

                cbAccountGroup1.ItemsSource = sub1;
                cbAccountGroup1.SelectedValue = "0";
                cbAccountGroup1.IsEnabled = true;
            }
        }

        private void cbAccountGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1)
            {
                if (sub1.Id == 0)
                    return;
                List<Account> sub2 =
                    [
                    new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 2",
                    },
                    ..Accounts
                    .Where(a => a.AccountTypeId == tipo.Id
                            && a.AccountSubGroupNumber1 == sub1.AccountSubGroupNumber1
                            && a.AccountSubGroupNumber2 != 0
                            && a.AccountSubGroupNumber3 == 0
                            && a.AccountSubGroupNumber4 == 0
                            && a.AccountSubGroupNumber5 == 0)
                    .GroupBy(c => c.AccountSubGroupNumber2)
                    .Select(g => g.First())
                    .OrderBy(c => c.AccountSubGroupNumber2)
                    .ToList(),
                ];

                cbAccountGroup2.ItemsSource = sub2;
                cbAccountGroup2.SelectedValue = "0";
                cbAccountGroup2.IsEnabled = true;
            }
        }

        private void cbAccountGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1
                && cbAccountGroup2.SelectedItem is Account sub2)
            {
                if (sub2.Id == 0)
                    return;
                List<Account> sub3 =
                    [
                    new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 3",
                    },
                    .. Accounts
                    .Where(a => a.AccountTypeId == tipo.Id
                            && a.AccountSubGroupNumber1 == sub1.AccountSubGroupNumber1
                            && a.AccountSubGroupNumber2 == sub2.AccountSubGroupNumber2
                            && a.AccountSubGroupNumber3 != 0
                            && a.AccountSubGroupNumber4 == 0
                            && a.AccountSubGroupNumber5 == 0)
                    .GroupBy(c => c.AccountSubGroupNumber3)
                    .Select(g => g.First())
                    .OrderBy(c => c.AccountSubGroupNumber3)
                    .ToList(),
                ];

                cbAccountGroup3.ItemsSource = sub3;
                cbAccountGroup3.SelectedValue = "0";
                cbAccountGroup3.IsEnabled = true;
            }
        }

        private void cbAccountGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1 &&
                cbAccountGroup2.SelectedItem is Account sub2 && cbAccountGroup3.SelectedItem is Account sub3)
            {
                if (sub3.Id == 0)
                    return;
                List<Account> sub4 =
                    [
                    new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 4",
                    },
                    .. Accounts
                    .Where(a => a.AccountTypeId == tipo.Id
                            && a.AccountSubGroupNumber1 == sub1.AccountSubGroupNumber1
                            && a.AccountSubGroupNumber2 == sub2.AccountSubGroupNumber2
                            && a.AccountSubGroupNumber3 == sub3.AccountSubGroupNumber3
                            && a.AccountSubGroupNumber4 != 0
                            && a.AccountSubGroupNumber5 == 0)
                    .GroupBy(c => c.AccountSubGroupNumber4)
                    .Select(g => g.First())
                    .OrderBy(c => c.AccountSubGroupNumber4)
                    .ToList(),
                ];

                cbAccountGroup4.ItemsSource = sub4;
                cbAccountGroup4.SelectedValue = "0";
                cbAccountGroup4.IsEnabled = true;
            }
        }

        private void cbAccountGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1
                && cbAccountGroup2.SelectedItem is Account sub2 && cbAccountGroup3.SelectedItem is Account sub3
                && cbAccountGroup4.SelectedItem is Account sub4)
            {
                if (sub4.Id == 0)
                    return;
                List<Account> sub5 =
                    [
                    new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 5",
                    },
                    .. Accounts
                    .Where(a => a.AccountTypeId == tipo.Id
                             && a.AccountSubGroupNumber1 == sub1.AccountSubGroupNumber1
                             && a.AccountSubGroupNumber2 == sub2.AccountSubGroupNumber2
                             && a.AccountSubGroupNumber3 == sub3.AccountSubGroupNumber3
                             && a.AccountSubGroupNumber4 == sub4.AccountSubGroupNumber4
                             && a.AccountSubGroupNumber5 != 0)
                    .GroupBy(c => c.AccountSubGroupNumber5)
                    .Select(g => g.First())
                    .OrderBy(c => c.AccountSubGroupNumber5)
                    .ToList(),
                ];

                cbAccountGroup5.ItemsSource = sub5;
                cbAccountGroup5.SelectedValue = "0";
                cbAccountGroup5.IsEnabled = true;
            }
        }
    }
}
