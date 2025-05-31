using GestionComercial.Desktop.Services;
using GestionComercial.Domain.DTOs.Accounts;
using GestionComercial.Domain.Entities.AccountingBook;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
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
        private AccountViewModel accountViewModel { get; set; } = new AccountViewModel();


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
                cbAccountGroup5.Visibility = Visibility.Hidden;
                lblAccountGroup5.Visibility = Visibility.Hidden;
            }
            else
            {
                GetAllAccountsAsync();
                cbAccountGroup5.Visibility = Visibility.Hidden;
                lblAccountGroup5.Visibility = Visibility.Hidden;
                accountViewModel.IsEnabled = true;
                accountViewModel.CreateDate = DateTime.Now;
                accountViewModel.CreateUser = App.UserName;
                DataContext = accountViewModel;
                btnAdd.Visibility = Visibility.Visible;
                btnUpdate.Visibility = Visibility.Hidden;
            }
        }

        private async void FindAccountAsync()
        {
            AccountResponse result = await _accountsApiService.GetByIdAsync(AccountId);
            if (result.Success)
            {
                AccountTypes = await _accountsApiService.GetAllAccountTypesAsync(true, false, false);
                Accounts = await _accountsApiService.GetAllAccountAsync(true, false, true);
                cbAccountType.ItemsSource = AccountTypes;
                cbAccountGroup1.ItemsSource = Accounts
                                                .Where(a => a.AccountTypeId == result.AccountViewModel.AccountTypeId
                                                        && a.AccountSubGroupNumber2 == 0
                                                        && a.AccountSubGroupNumber2 == 0
                                                        && a.AccountSubGroupNumber3 == 0
                                                        && a.AccountSubGroupNumber4 == 0
                                                        && a.AccountSubGroupNumber5 == 0)
                                                .GroupBy(c => c.AccountSubGroupNumber1)
                                                .Select(g => g.First())
                                                .OrderBy(c => c.AccountSubGroupNumber1)
                                                .ToList();
                cbAccountGroup2.ItemsSource = Accounts
                                                .Where(a => a.AccountTypeId == result.AccountViewModel.AccountTypeId
                                                        && a.AccountSubGroupNumber1 == result.AccountViewModel.AccountSubGroupNumber1
                                                        && a.AccountSubGroupNumber2 != 0
                                                        && a.AccountSubGroupNumber3 == 0
                                                        && a.AccountSubGroupNumber4 == 0
                                                        && a.AccountSubGroupNumber5 == 0)
                                                .GroupBy(c => c.AccountSubGroupNumber2)
                                                .Select(g => g.First())
                                                .OrderBy(c => c.AccountSubGroupNumber2)
                                                .ToList();
                cbAccountGroup3.ItemsSource = Accounts
                                                .Where(a => a.AccountTypeId == result.AccountViewModel.AccountTypeId
                                                        && a.AccountSubGroupNumber1 == result.AccountViewModel.AccountSubGroupNumber1
                                                        && a.AccountSubGroupNumber2 == result.AccountViewModel.AccountSubGroupNumber2
                                                        && a.AccountSubGroupNumber3 != 0
                                                        && a.AccountSubGroupNumber4 == 0
                                                        && a.AccountSubGroupNumber5 == 0)
                                                .GroupBy(c => c.AccountSubGroupNumber3)
                                                .Select(g => g.First())
                                                .OrderBy(c => c.AccountSubGroupNumber3)
                                                .ToList();
                cbAccountGroup4.ItemsSource = Accounts
                                                .Where(a => a.AccountTypeId == result.AccountViewModel.AccountTypeId
                                                        && a.AccountSubGroupNumber1 == result.AccountViewModel.AccountSubGroupNumber1
                                                        && a.AccountSubGroupNumber2 == result.AccountViewModel.AccountSubGroupNumber2
                                                        && a.AccountSubGroupNumber3 == result.AccountViewModel.AccountSubGroupNumber3
                                                        && a.AccountSubGroupNumber4 != 0
                                                        && a.AccountSubGroupNumber5 == 0)
                                                .GroupBy(c => c.AccountSubGroupNumber4)
                                                .Select(g => g.First())
                                                .OrderBy(c => c.AccountSubGroupNumber4)
                                                .ToList();
                cbAccountGroup5.ItemsSource = Accounts
                                                .Where(a => a.AccountTypeId == result.AccountViewModel.AccountTypeId
                                                        && a.AccountSubGroupNumber1 == result.AccountViewModel.AccountSubGroupNumber1
                                                        && a.AccountSubGroupNumber2 == result.AccountViewModel.AccountSubGroupNumber2
                                                        && a.AccountSubGroupNumber3 == result.AccountViewModel.AccountSubGroupNumber3
                                                        && a.AccountSubGroupNumber4 == result.AccountViewModel.AccountSubGroupNumber4
                                                        && a.AccountSubGroupNumber5 != 0)
                                                .GroupBy(c => c.AccountSubGroupNumber5)
                                                .Select(g => g.First())
                                                .OrderBy(c => c.AccountSubGroupNumber5)
                                                .ToList();
                cbAccountGroup1.IsEnabled = true;
                cbAccountGroup2.IsEnabled = true;
                cbAccountGroup3.IsEnabled = true;
                cbAccountGroup4.IsEnabled = true;
                cbAccountGroup5.IsEnabled = true;
                accountViewModel = result.AccountViewModel;
                cbAccountType.SelectedValue = result.AccountViewModel.AccountTypeId.ToString();
                cbAccountGroup1.SelectedValue = result.AccountViewModel.AccountIdSubGroupNumber1.ToString();
                cbAccountGroup2.SelectedValue = result.AccountViewModel.AccountIdSubGroupNumber2.ToString();
                cbAccountGroup3.SelectedValue = result.AccountViewModel.AccountIdSubGroupNumber3.ToString();
                cbAccountGroup4.SelectedValue = result.AccountViewModel.AccountIdSubGroupNumber4.ToString();
                //cbAccountGroup5.SelectedValue = result.AccountViewModel.AccountIdSubGroupNumber5.ToString();


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
                accountViewModel.Name = accountViewModel.Name.ToUpper();
                accountViewModel.UpdateUser = App.UserName;
                accountViewModel.UpdateDate = DateTime.Now;
                accountViewModel.AccountIdSubGroupNumber1 = Convert.ToInt32(cbAccountGroup1.SelectedValue);
                accountViewModel.AccountIdSubGroupNumber2 = Convert.ToInt32(cbAccountGroup2.SelectedValue);
                accountViewModel.AccountIdSubGroupNumber3 = Convert.ToInt32(cbAccountGroup3.SelectedValue);
                accountViewModel.AccountIdSubGroupNumber4 = Convert.ToInt32(cbAccountGroup4.SelectedValue);
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
            accountViewModel.Name = accountViewModel.Name.ToUpper();
            accountViewModel.AccountIdSubGroupNumber1 = Convert.ToInt32(cbAccountGroup1.SelectedValue);
            accountViewModel.AccountIdSubGroupNumber2 = Convert.ToInt32(cbAccountGroup2.SelectedValue);
            accountViewModel.AccountIdSubGroupNumber3 = Convert.ToInt32(cbAccountGroup3.SelectedValue);
            accountViewModel.AccountIdSubGroupNumber4 = Convert.ToInt32(cbAccountGroup4.SelectedValue);
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
            if (AccountId == 0)
                if (cbAccountType.SelectedItem is AccountType tipo)
                {
                    if (tipo.Id == 0)
                    {
                        cbAccountGroup1.Visibility = Visibility.Hidden;
                        cbAccountGroup2.Visibility = Visibility.Hidden;
                        cbAccountGroup3.Visibility = Visibility.Hidden;
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup1.Visibility = Visibility.Hidden;
                        lblAccountGroup2.Visibility = Visibility.Hidden;
                        lblAccountGroup3.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        return;
                    }
                    List<Account> sub1 =
                    [
                        new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 1",
                    },
                    new Account {
                         Id= -1,
                         Name = "Nueva SubCuenta 1",
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

                    if (sub1.Count > 2)
                    {
                        cbAccountGroup1.Visibility = Visibility.Visible;
                        lblAccountGroup1.Visibility = Visibility.Visible;
                        cbAccountGroup1.ItemsSource = sub1;
                        if (AccountId == 0)
                            cbAccountGroup1.SelectedValue = "0";
                        cbAccountGroup1.IsEnabled = true;
                    }
                    else
                    {
                        cbAccountGroup1.Visibility = Visibility.Hidden;
                        cbAccountGroup2.Visibility = Visibility.Hidden;
                        cbAccountGroup3.Visibility = Visibility.Hidden;
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup1.Visibility = Visibility.Hidden;
                        lblAccountGroup2.Visibility = Visibility.Hidden;
                        lblAccountGroup3.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber1 = 1;
                        accountViewModel.AccountSubGroupNumber2 = 0;
                        accountViewModel.AccountSubGroupNumber3 = 0;
                        accountViewModel.AccountSubGroupNumber4 = 0;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                    }
                }
        }

        private void cbAccountGroup1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountId == 0)
                if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1)
                {
                    if (sub1.Id == 0)
                        return;
                    else if (sub1.Id == -1)
                    {
                        cbAccountGroup2.Visibility = Visibility.Hidden;
                        cbAccountGroup3.Visibility = Visibility.Hidden;
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup2.Visibility = Visibility.Hidden;
                        lblAccountGroup3.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber1 = Accounts.Where(a => a.AccountTypeId == tipo.Id).Max(a => a.AccountSubGroupNumber1) + 1;
                        accountViewModel.AccountSubGroupNumber2 = 0;
                        accountViewModel.AccountSubGroupNumber3 = 0;
                        accountViewModel.AccountSubGroupNumber4 = 0;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                        return;
                    }
                    List<Account> sub2 =
                    [
                        new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 2",
                    },
                    new Account {
                         Id= -1,
                         Name = "Nueva SubCuenta 2",
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

                    if (sub2.Count > 2)
                    {
                        cbAccountGroup2.Visibility = Visibility.Visible;
                        lblAccountGroup2.Visibility = Visibility.Visible;
                        cbAccountGroup2.ItemsSource = sub2;
                        if (AccountId == 0)
                            cbAccountGroup2.SelectedValue = "0";
                        cbAccountGroup2.IsEnabled = true;
                    }
                    else
                    {
                        cbAccountGroup2.Visibility = Visibility.Hidden;
                        cbAccountGroup3.Visibility = Visibility.Hidden;
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup2.Visibility = Visibility.Hidden;
                        lblAccountGroup3.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber2 = 1;
                        accountViewModel.AccountSubGroupNumber3 = 0;
                        accountViewModel.AccountSubGroupNumber4 = 0;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                    }
                }
        }

        private void cbAccountGroup2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountId == 0)
                if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1
                && cbAccountGroup2.SelectedItem is Account sub2)
                {
                    if (sub2.Id == 0)
                        return;
                    else if (sub2.Id == -1)
                    {
                        cbAccountGroup3.Visibility = Visibility.Hidden;
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup3.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber1 = sub1.AccountSubGroupNumber1;
                        accountViewModel.AccountSubGroupNumber2 = Accounts
                            .Where(a => a.AccountTypeId == tipo.Id
                                && a.AccountSubGroupNumber1 == sub1.AccountSubGroupNumber1)
                            .Max(a => a.AccountSubGroupNumber2) + 1;
                        accountViewModel.AccountSubGroupNumber3 = 0;
                        accountViewModel.AccountSubGroupNumber4 = 0;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                        return;
                    }
                    List<Account> sub3 =
                        [
                        new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 3",
                    },
                    new Account {
                         Id= -1,
                         Name = "Nueva SubCuenta 3",
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

                    if (sub3.Count > 2)
                    {
                        cbAccountGroup3.Visibility = Visibility.Visible;
                        lblAccountGroup3.Visibility = Visibility.Visible;
                        cbAccountGroup3.ItemsSource = sub3;
                        if (AccountId == 0)
                            cbAccountGroup3.SelectedValue = "0";
                        cbAccountGroup3.IsEnabled = true;
                    }
                    else
                    {
                        cbAccountGroup3.Visibility = Visibility.Hidden;
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup3.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber3 = 1;
                        accountViewModel.AccountSubGroupNumber4 = 0;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                    }
                }
        }

        private void cbAccountGroup3_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountId == 0)
                if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1 &&
                cbAccountGroup2.SelectedItem is Account sub2 && cbAccountGroup3.SelectedItem is Account sub3)
                {
                    if (sub3.Id == 0)
                        return;
                    else if (sub3.Id == -1)
                    {
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber1 = sub2.AccountSubGroupNumber1;
                        accountViewModel.AccountSubGroupNumber2 = sub2.AccountSubGroupNumber2;
                        accountViewModel.AccountSubGroupNumber3 = Accounts
                            .Where(a => a.AccountTypeId == tipo.Id
                                && a.AccountSubGroupNumber1 == sub2.AccountSubGroupNumber1
                                && a.AccountSubGroupNumber2 == sub2.AccountSubGroupNumber2)
                            .Max(a => a.AccountSubGroupNumber3) + 1;
                        accountViewModel.AccountSubGroupNumber4 = 0;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                        return;
                    }
                    List<Account> sub4 =
                        [
                        new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 4",
                    },
                    new Account {
                         Id= -1,
                         Name = "Nueva SubCuenta 4",
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

                    if (sub4.Count > 2)
                    {
                        cbAccountGroup4.Visibility = Visibility.Visible;
                        lblAccountGroup4.Visibility = Visibility.Visible;
                        cbAccountGroup4.ItemsSource = sub4;
                        if (AccountId == 0)
                            cbAccountGroup4.SelectedValue = "0";
                        cbAccountGroup4.IsEnabled = true;
                    }
                    else
                    {
                        cbAccountGroup4.Visibility = Visibility.Hidden;
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup4.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;
                        accountViewModel.AccountSubGroupNumber4 = 1;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                    }
                }
        }

        private void cbAccountGroup4_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountId == 0)
                if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1
                && cbAccountGroup2.SelectedItem is Account sub2 && cbAccountGroup3.SelectedItem is Account sub3
                && cbAccountGroup4.SelectedItem is Account sub4)
                {
                    if (sub4.Id == 0)
                        return;
                    else if (sub4.Id == -1)
                    {
                        cbAccountGroup5.Visibility = Visibility.Hidden;
                        lblAccountGroup5.Visibility = Visibility.Hidden;

                        accountViewModel.AccountSubGroupNumber1 = sub3.AccountSubGroupNumber1;
                        accountViewModel.AccountSubGroupNumber2 = sub3.AccountSubGroupNumber2;
                        accountViewModel.AccountSubGroupNumber3 = sub3.AccountSubGroupNumber3;
                        accountViewModel.AccountSubGroupNumber4 = Accounts
                            .Where(a => a.AccountTypeId == tipo.Id
                                && a.AccountSubGroupNumber1 == sub3.AccountSubGroupNumber1
                                && a.AccountSubGroupNumber2 == sub3.AccountSubGroupNumber2
                                && a.AccountSubGroupNumber3 == sub3.AccountSubGroupNumber3)
                            .Max(a => a.AccountSubGroupNumber4) + 1;
                        accountViewModel.AccountSubGroupNumber5 = 0;
                        return;
                    }
                    List<Account> sub5 =
                        [
                        new Account
                    {
                        Id = 0,
                        Name = "Seleccione la SubCuenta 5",
                    },
                    new Account {
                         Id= -1,
                         Name = "Nueva SubCuenta 5",
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
                    if (AccountId == 0)
                        cbAccountGroup5.SelectedValue = "0";
                    cbAccountGroup5.IsEnabled = true;

                }
        }

        private void cbAccountGroup5_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AccountId == 0)
                if (cbAccountType.SelectedItem is AccountType tipo && cbAccountGroup1.SelectedItem is Account sub1
                   && cbAccountGroup2.SelectedItem is Account sub2 && cbAccountGroup3.SelectedItem is Account sub3
                   && cbAccountGroup4.SelectedItem is Account sub4 && cbAccountGroup5.SelectedItem is Account sub5)
                {
                    if (sub5.Id == 0)
                        return;
                    else if (sub5.Id == -1)
                    {
                        accountViewModel.AccountSubGroupNumber1 = sub4.AccountSubGroupNumber1;
                        accountViewModel.AccountSubGroupNumber2 = sub4.AccountSubGroupNumber2;
                        accountViewModel.AccountSubGroupNumber3 = sub4.AccountSubGroupNumber3;
                        accountViewModel.AccountSubGroupNumber4 = sub4.AccountSubGroupNumber4;

                        accountViewModel.AccountSubGroupNumber5 = Accounts
                            .Where(a => a.AccountTypeId == tipo.Id
                                && a.AccountSubGroupNumber1 == sub4.AccountSubGroupNumber1
                                && a.AccountSubGroupNumber2 == sub4.AccountSubGroupNumber2
                                && a.AccountSubGroupNumber3 == sub4.AccountSubGroupNumber3
                                && a.AccountSubGroupNumber4 == sub4.AccountSubGroupNumber4)
                            .Max(a => a.AccountSubGroupNumber5) + 1;
                        return;
                    }
                }
        }
    }
}
