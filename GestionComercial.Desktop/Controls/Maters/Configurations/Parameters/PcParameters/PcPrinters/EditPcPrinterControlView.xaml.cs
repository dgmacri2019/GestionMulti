using GestionComercial.Desktop.Services;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Master.Configurations.PcParameters;
using GestionComercial.Domain.Helpers;
using GestionComercial.Domain.Response;
using System.Windows;
using System.Windows.Controls;

namespace GestionComercial.Desktop.Controls.Maters.Configurations.Parameters.PcParameters.PcPrinters
{
    /// <summary>
    /// Lógica de interacción para EditPcPrinterControlView.xaml
    /// </summary>
    public partial class EditPcPrinterControlView : UserControl
    {
        private readonly ParametersApiService _parametersApiService;
        private PcPrinterParametersListViewModel? PrinterParameterViewModel { get; set; }
        //private PcPrinterParameter PcPrinterParameter { get; set; }

        public event Action ImpresorasActualizadas;
        public EditPcPrinterControlView()
        {
            InitializeComponent();
            ImpresorasActualizadas?.Invoke();
            _parametersApiService = new ParametersApiService();
            PrinterParameterViewModel = ParameterCache.Instance.GetPrinterParameter();
            if (PrinterParameterViewModel == null)
                PrinterParameterViewModel = new PcPrinterParametersListViewModel
                {
                    CreateDate = DateTime.Now,
                    CreateUser = LoginUserCache.UserName,
                    ComputerName = Environment.MachineName,
                    SalePoint = ParameterCache.Instance.GetPcParameter().SalePoint,
                    IsDeleted = false,
                    IsEnabled = true,
                };
            else
            {
                PrinterParameterViewModel.UpdateDate = DateTime.Now;
                PrinterParameterViewModel.UpdateUser = LoginUserCache.UserName;

            }
            PrinterParameterViewModel.LoadInstalledPrinters();
            DataContext = PrinterParameterViewModel;
        }


        private void miUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            lblError.MaxWidth = this.ActualWidth;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ImpresorasActualizadas?.Invoke();
        }

        private async void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblError.Text = string.Empty;
                lblConfirm.Text = string.Empty;
                btnUpdate.IsEnabled = false;


                GeneralResponse resultUpdate = await _parametersApiService.UpdatePcPrinterParameterAsync(ConverterHelper.ToPrinterParameter(PrinterParameterViewModel, PrinterParameterViewModel.Id == 0));
                if (resultUpdate.Success)
                {
                    lblConfirm.Text = resultUpdate.Message;
                    ImpresorasActualizadas?.Invoke();                    
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




        private void msgError(string msg)
        {
            lblError.Text = msg;
        }
    }
}
