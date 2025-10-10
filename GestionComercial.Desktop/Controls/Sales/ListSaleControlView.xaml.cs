using GestionComercial.Desktop.Helpers;
using GestionComercial.Desktop.Services;
using GestionComercial.Desktop.ViewModels.Sale;
using GestionComercial.Desktop.Views.Sales;
using GestionComercial.Domain.Cache;
using GestionComercial.Domain.DTOs.Sale;
using GestionComercial.Domain.Response;
using System.Drawing.Printing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using static GestionComercial.Domain.Constant.Enumeration;

namespace GestionComercial.Desktop.Controls.Sales
{
    /// <summary>
    /// Lógica de interacción para ListSaleControlView.xaml
    /// </summary>
    public partial class ListSaleControlView : UserControl
    {
        private readonly SalesApiService _salesApiService;
        private readonly InvoicesApiService _invoicesApiService;

        private bool _isLoaded = false;

        public ListSaleControlView()
        {
            InitializeComponent();
            btnAdd.Visibility = AutorizeOperationHelper.ValidateOperation(ModuleType.Sales, "Ventas-Agregar") ? Visibility.Visible : Visibility.Collapsed;
            _salesApiService = new SalesApiService();
            _invoicesApiService = new InvoicesApiService();
            DataContext = new SaleListViewModel();
            dpDate.DisplayDateEnd = DateTime.Now;
            dpDate.SelectedDate = DateTime.Now;
            _isLoaded = true;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saleAddWindow = new SaleAddWindow(0) { Owner = Window.GetWindow(this) };
                saleAddWindow.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Aviso al operador", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void DataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }
        private void dpDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpDate.SelectedDate.HasValue && _isLoaded)
            {
               
            }
        }

        private async void BtnConvertInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int saleId)
            {
                InvoiceResponse invoiceResponse = await _invoicesApiService.AddAsync(saleId);
                if (invoiceResponse.Success)
                    Print(invoiceResponse.Bytes, true);
                else
                    MsgBoxAlertHelper.MsgAlertError(invoiceResponse.Message);
            }
        }

        private async void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int saleId)
            {
                SaleResponse saleResponse = await _salesApiService.PrintAsync(saleId);
                if (saleResponse.Success)
                    Print(saleResponse.Bytes, false);
                else
                    MsgBoxAlertHelper.MsgAlertError(saleResponse.Message);
            }

        }

        private async void BtnAnull_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int saleId)
            {
                SaleViewModel? sale = SaleCache.Instance.FindById(saleId);

                if (sale != null)
                {
                    if (!sale.HasCAE)
                    {
                        if (MessageBox.Show("Confirma la anulación de la proforma " + sale.SaleNumberString, "Anular Comprobante", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            SaleResponse resultAnular = await _salesApiService.AnullAsync(saleId, ParameterCache.Instance.GetPcParameter().SalePoint);
                            if (resultAnular.Success)
                                Print(resultAnular.Bytes, false);
                            else
                                MsgBoxAlertHelper.MsgAlertError(resultAnular.Message);
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Confirma la anulación de la factura " + sale.InvoiceNumber, "Anular Comprobante", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                        {
                            InvoiceResponse resultAnular = await _invoicesApiService.AnullAsync(saleId, ParameterCache.Instance.GetPcParameter().SalePoint);
                            if (resultAnular.Success)
                                Print(resultAnular.Bytes, true);
                            else
                                MsgBoxAlertHelper.MsgAlertError(resultAnular.Message);
                        }
                    }
                }



            }

        }

        private void Print(byte[] bytes, bool isInvoice)
        {
            try
            {
                if (isInvoice && ParameterCache.Instance.HasDataPcPrinterParameter && ParameterCache.Instance.GetPrinterParameter().EnablePrintInvoice)
                {
                    string printerName = ParameterCache.Instance.GetPrinterParameter().InvoicePrinter;

                    using (var ms = new MemoryStream(bytes))
                    using (var document = PdfiumViewer.PdfDocument.Load(ms))
                    {
                        using (var printDocument = document.CreatePrintDocument())
                        {
                            printDocument.PrinterSettings = new PrinterSettings
                            {
                                PrinterName = printerName
                            };

                            printDocument.Print();
                        }
                    }
                }
                else if (ParameterCache.Instance.HasDataPcPrinterParameter && ParameterCache.Instance.GetPrinterParameter().EnablePrintSale)
                {
                    string printerName = ParameterCache.Instance.GetPrinterParameter().SalePrinter;

                    using (var ms = new MemoryStream(bytes))
                    using (var document = PdfiumViewer.PdfDocument.Load(ms))
                    {
                        using (var printDocument = document.CreatePrintDocument())
                        {
                            printDocument.PrinterSettings = new PrinterSettings
                            {
                                PrinterName = printerName
                            };

                            printDocument.Print();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBoxAlertHelper.MsgAlertError(ex.Message);
            }
        }

        
    }
}
