using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Banks;
using GestionComercial.Domain.Entities.BoxAndBank;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GestionComercial.Desktop.Controls.Banks
{
    /// <summary>
    /// Lógica de interacción para EditBoxControlView.xaml
    /// </summary>
    public partial class EditBoxControlView : UserControl
    {
        private readonly BanksApiService _bankApiService;
        private readonly int BoxId;
        private BoxViewModel BoxViewModel { get; set; }

        public event Action CajaActualizada;

        public EditBoxControlView(int boxId)
        {
            InitializeComponent();
            _bankApiService = new BanksApiService();
            BoxId = boxId;
            FindBox();
            if (BoxId > 0)
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
        private async void FindBox()
        {
            BankAndBoxResponse result = await _bankApiService.GetBoxByIdAsync(BoxId, true, false);
            if (result.Success)
            {
                BoxViewModel = result.BoxViewModel;
                if (BoxId == 0)
                {
                    BoxViewModel.CreateUser = LoginUserCache.UserName;
                }
                DataContext = BoxViewModel;
            }
            else
                lblError.Text = result.Message;
        }

        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblError.MaxWidth = this.ActualWidth;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                lblError.Text = string.Empty;
                btnAdd.IsEnabled = false;
                lblError.Text = string.Empty;
                BoxViewModel.CreateUser = LoginUserCache.UserName;
                BoxViewModel.BoxName = BoxViewModel.BoxName.ToUpper();

                Box box = ConverterHelper.ToBox(BoxViewModel, BoxViewModel.Id == 0);
                GeneralResponse resultUpdate = await _bankApiService.AddBoxAsync(box);
                if (resultUpdate.Success)
                    CajaActualizada?.Invoke(); // para notificar a la vista principal
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
                BoxViewModel.UpdateUser = LoginUserCache.UserName;
                BoxViewModel.BoxName = BoxViewModel.BoxName.ToUpper();
                BoxViewModel.UpdateDate = DateTime.Now;

                Box box = ConverterHelper.ToBox(BoxViewModel, BoxViewModel.Id == 0);
                GeneralResponse resultUpdate = await _bankApiService.UpdateBoxAsync(box);
                if (resultUpdate.Success)
                    CajaActualizada?.Invoke(); // para notificar a la vista principal
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
            CajaActualizada?.Invoke(); // para notificar a la vista principal
        }

        private void txtSold_LostFocus(object sender, RoutedEventArgs e)
        {
            txtSold.Text = txtSold.Text.Replace(".", ",");
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
    }
}
