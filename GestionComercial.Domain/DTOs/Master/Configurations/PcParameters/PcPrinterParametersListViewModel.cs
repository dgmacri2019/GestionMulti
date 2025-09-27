using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace GestionComercial.Domain.DTOs.Master.Configurations.PcParameters
{
    public class PcPrinterParametersListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // ================== Datos base ==================
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Display(Name = "Creado el")]
        public DateTime CreateDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Creado por")]
        public string CreateUser { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm}", ApplyFormatInEditMode = true)]
        [Display(Name = "Modificado el")]
        public DateTime? UpdateDate { get; set; }

        [MaxLength(100, ErrorMessage = "El campo {0} no puede contener más de {1} caracteres")]
        [Display(Name = "Modificado por")]
        public string? UpdateUser { get; set; }

        [Display(Name = "Borrado?")]
        public bool IsDeleted { get; set; }

        [Display(Name = "Habilitado?")]
        public bool IsEnabled { get; set; }

        public bool IsEnabledOtherPrinters => !UseAllPrinters;

        // ================== Impresora Factura ==================
        private string? _invoicePrinter;
        public string? InvoicePrinter
        {
            get => _invoicePrinter;
            set
            {
                if (_invoicePrinter != value)
                {
                    _invoicePrinter = value;
                    OnPropertyChanged(nameof(InvoicePrinter));
                    // 🔹 Si está en modo "Única impresora", propagar al resto
                    if (UseAllPrinters && !string.IsNullOrEmpty(_invoicePrinter))
                    {
                        RemitPrinter = _invoicePrinter;
                        BudgetPrinter = _invoicePrinter;
                        OrderPrinter = _invoicePrinter;
                        BarCodePrinter = _invoicePrinter;
                        TicketChangePrinter = _invoicePrinter;
                        SalePrinter = _invoicePrinter;
                    }
                }
            }
        }

        private int _maxWidthInvoicePrinter = 210;
        public int MaxWidthInvoicePrinter
        {
            get => _maxWidthInvoicePrinter;
            set
            {
                if (_maxWidthInvoicePrinter != value)
                {
                    _maxWidthInvoicePrinter = value;
                    OnPropertyChanged(nameof(MaxWidthInvoicePrinter));
                }
            }
        }

        private bool _useContinuousInvoicePrinter;
        public bool UseContinuousInvoicePrinter
        {
            get => _useContinuousInvoicePrinter;
            set
            {
                if (_useContinuousInvoicePrinter != value)
                {
                    _useContinuousInvoicePrinter = value;
                    OnPropertyChanged(nameof(UseContinuousInvoicePrinter));

                    if (UseAllPrinters)
                        RaiseAllEffectiveThermalProperties();   // 🔹 agregado
                       
                    // 🔹 Propagar el ancho al resto
                    MaxWidthRemitPrinter = value ? 80 : 210;
                    MaxWidthBudgetPrinter = value ? 80 : 210;
                    MaxWidthOrderPrinter = value ? 80 : 210;
                    MaxWidthBarCodePrinter = value ? 80 : 210;
                    MaxWidthTicketChangePrinter = value ? 80 : 210;
                    MaxWidthSalePrinter = value ? 80 : 210;
                    MaxWidthInvoicePrinter = value ? 80 : 210;
                }
            }
        }

        private bool _enablePrintInvoice;
        public bool EnablePrintInvoice
        {
            get => _enablePrintInvoice;
            set
            {
                if (_enablePrintInvoice != value)
                {
                    _enablePrintInvoice = value;
                    OnPropertyChanged(nameof(EnablePrintInvoice));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersInvoice { get; set; } = new();

        // ================== Impresora Remito ==================
        private string? _remitPrinter;
        public string? RemitPrinter
        {
            get => _remitPrinter;
            set
            {
                if (_remitPrinter != value)
                {
                    _remitPrinter = value;
                    OnPropertyChanged(nameof(RemitPrinter));
                }
            }
        }

        private int _maxWidthRemitPrinter = 210;
        public int MaxWidthRemitPrinter
        {
            get => _maxWidthRemitPrinter;
            set
            {
                if (_maxWidthRemitPrinter != value)
                {
                    _maxWidthRemitPrinter = value;
                    OnPropertyChanged(nameof(MaxWidthRemitPrinter));
                }
            }
        }

        private bool _useContinuousRemitPrinter;
        public bool UseContinuousRemitPrinter
        {
            get => _useContinuousRemitPrinter;
            set
            {
                if (_useContinuousRemitPrinter != value)
                {
                    _useContinuousRemitPrinter = value;
                    OnPropertyChanged(nameof(UseContinuousRemitPrinter));
                    OnPropertyChanged(nameof(UseContinuousRemitPrinterEffective));
                }
                MaxWidthRemitPrinter = value ? 80 : 210;
            }
        }

        public bool UseContinuousRemitPrinterEffective =>
            UseAllPrinters ? UseContinuousInvoicePrinter : UseContinuousRemitPrinter;

        private bool _enablePrintRemit;
        public bool EnablePrintRemit
        {
            get => _enablePrintRemit;
            set
            {
                if (_enablePrintRemit != value)
                {
                    _enablePrintRemit = value;
                    OnPropertyChanged(nameof(EnablePrintRemit));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersRemit { get; set; } = new();

        // ================== Impresora Presupuesto ==================
        private string? _budgetPrinter;
        public string? BudgetPrinter
        {
            get => _budgetPrinter;
            set
            {
                if (_budgetPrinter != value)
                {
                    _budgetPrinter = value;
                    OnPropertyChanged(nameof(BudgetPrinter));
                }
            }
        }

        private int _maxWidthBudgetPrinter = 210;
        public int MaxWidthBudgetPrinter
        {
            get => _maxWidthBudgetPrinter;
            set
            {
                if (_maxWidthBudgetPrinter != value)
                {
                    _maxWidthBudgetPrinter = value;
                    OnPropertyChanged(nameof(MaxWidthBudgetPrinter));
                }
            }
        } 

        private bool _useContinuousBudgetPrinter;
        public bool UseContinuousBudgetPrinter
        {
            get => _useContinuousBudgetPrinter;
            set
            {
                if (_useContinuousBudgetPrinter != value)
                {
                    _useContinuousBudgetPrinter = value;
                    OnPropertyChanged(nameof(UseContinuousBudgetPrinter));
                    OnPropertyChanged(nameof(UseContinuousBudgetPrinterEffective));
                }
                MaxWidthBudgetPrinter = value ? 80 : 210;
            }
        }

        public bool UseContinuousBudgetPrinterEffective =>
            UseAllPrinters ? UseContinuousInvoicePrinter : UseContinuousBudgetPrinter;

        private bool _enablePrintBudget;
        public bool EnablePrintBudget
        {
            get => _enablePrintBudget;
            set
            {
                if (_enablePrintBudget != value)
                {
                    _enablePrintBudget = value;
                    OnPropertyChanged(nameof(EnablePrintBudget));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersBudget { get; set; } = new();

        // ================== Impresora Pedido ==================
        private string? _orderPrinter;
        public string? OrderPrinter
        {
            get => _orderPrinter;
            set
            {
                if (_orderPrinter != value)
                {
                    _orderPrinter = value;
                    OnPropertyChanged(nameof(OrderPrinter));
                }
            }
        }

        private int _maxWidthOrderPrinter = 210;
        public int MaxWidthOrderPrinter
        {
            get => _maxWidthOrderPrinter;
            set
            {
                if (_maxWidthOrderPrinter != value)
                {
                    _maxWidthOrderPrinter = value;
                    OnPropertyChanged(nameof(MaxWidthOrderPrinter));
                }
            }
        }

        private bool _useContinuousOrderPrinter;
        public bool UseContinuousOrderPrinter
        {
            get => _useContinuousOrderPrinter;
            set
            {
                if (_useContinuousOrderPrinter != value)
                {
                    _useContinuousOrderPrinter = value;
                    OnPropertyChanged(nameof(UseContinuousOrderPrinter));
                    OnPropertyChanged(nameof(UseContinuousOrderPrinterEffective));
                }
                MaxWidthOrderPrinter = value ? 80 : 210;
            }
        }

        public bool UseContinuousOrderPrinterEffective =>
            UseAllPrinters ? UseContinuousInvoicePrinter : UseContinuousOrderPrinter;

        private bool _enablePrintOrder;
        public bool EnablePrintOrder
        {
            get => _enablePrintOrder;
            set
            {
                if (_enablePrintOrder != value)
                {
                    _enablePrintOrder = value;
                    OnPropertyChanged(nameof(EnablePrintOrder));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersOrder { get; set; } = new();

        // ================== Impresora Código de Barra ==================
        private string? _barCodePrinter;
        public string? BarCodePrinter
        {
            get => _barCodePrinter;
            set
            {
                if (_barCodePrinter != value)
                {
                    _barCodePrinter = value;
                    OnPropertyChanged(nameof(BarCodePrinter));
                }
            }
        }

        private int _maxWidthBarCodePrinter = 210;
        public int MaxWidthBarCodePrinter
        {
            get => _maxWidthBarCodePrinter;
            set
            {
                if (_maxWidthBarCodePrinter != value)
                {
                    _maxWidthBarCodePrinter = value;
                    OnPropertyChanged(nameof(MaxWidthBarCodePrinter));
                }
            }
        }

        private bool _useContinuousBarCodePrinter;
        public bool UseContinuousBarCodePrinter
        {
            get => _useContinuousBarCodePrinter;
            set
            {
                if (_useContinuousBarCodePrinter != value)
                {
                    _useContinuousBarCodePrinter = value;
                    OnPropertyChanged(nameof(UseContinuousBarCodePrinter));
                    OnPropertyChanged(nameof(UseContinuousBarCodePrinterEffective));
                }
                MaxWidthBarCodePrinter = value ? 80 : 210;
            }
        }

        public bool UseContinuousBarCodePrinterEffective =>
            UseAllPrinters ? UseContinuousInvoicePrinter : UseContinuousBarCodePrinter;

        private bool _enablePrintBarCode;
        public bool EnablePrintBarCode
        {
            get => _enablePrintBarCode;
            set
            {
                if (_enablePrintBarCode != value)
                {
                    _enablePrintBarCode = value;
                    OnPropertyChanged(nameof(EnablePrintBarCode));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersBarCode { get; set; } = new();

        // ================== Impresora Ticket Cambio ==================
        private string? _ticketChangePrinter;
        public string? TicketChangePrinter
        {
            get => _ticketChangePrinter;
            set
            {
                if (_ticketChangePrinter != value)
                {
                    _ticketChangePrinter = value;
                    OnPropertyChanged(nameof(TicketChangePrinter));
                }
            }
        }

        private int _maxWidthTicketChangePrinter = 210;
        public int MaxWidthTicketChangePrinter
        {
            get => _maxWidthTicketChangePrinter;
            set
            {
                if (_maxWidthTicketChangePrinter != value)
                {
                    _maxWidthTicketChangePrinter = value;
                    OnPropertyChanged(nameof(MaxWidthTicketChangePrinter));
                }
            }
        }

        private bool _useContinuousTicketChangePrinter;
        public bool UseContinuousTicketChangePrinter
        {
            get => _useContinuousTicketChangePrinter;
            set
            {
                if (_useContinuousTicketChangePrinter != value)
                {
                    _useContinuousTicketChangePrinter = value;
                    OnPropertyChanged(nameof(UseContinuousTicketChangePrinter));
                    OnPropertyChanged(nameof(UseContinuousTicketChangePrinterEffective));
                }
                MaxWidthTicketChangePrinter = value ? 80 : 210;
            }
        }

        public bool UseContinuousTicketChangePrinterEffective =>
            UseAllPrinters ? UseContinuousInvoicePrinter : UseContinuousTicketChangePrinter;

        private bool _enablePrintTicketChange;
        public bool EnablePrintTicketChange
        {
            get => _enablePrintTicketChange;
            set
            {
                if (_enablePrintTicketChange != value)
                {
                    _enablePrintTicketChange = value;
                    OnPropertyChanged(nameof(EnablePrintTicketChange));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersTicketChange { get; set; } = new();

        // ================== Impresora Venta ==================
        private string? _salePrinter;
        public string? SalePrinter
        {
            get => _salePrinter;
            set
            {
                if (_salePrinter != value)
                {
                    _salePrinter = value;
                    OnPropertyChanged(nameof(SalePrinter));
                }
            }
        }

        private int _maxWidthSalePrinter = 210;
        public int MaxWidthSalePrinter
        {
            get => _maxWidthSalePrinter;
            set
            {
                if (_maxWidthSalePrinter != value)
                {
                    _maxWidthSalePrinter = value;
                    OnPropertyChanged(nameof(MaxWidthSalePrinter));
                }
            }
        }

        private bool _useContinuousSalePrinter;
        public bool UseContinuousSalePrinter
        {
            get => _useContinuousSalePrinter;
            set
            {
                if (_useContinuousSalePrinter != value)
                {
                    _useContinuousSalePrinter = value;
                    OnPropertyChanged(nameof(UseContinuousSalePrinter));
                    OnPropertyChanged(nameof(UseContinuousSalePrinterEffective));
                }
                MaxWidthSalePrinter = value ? 80 : 210;
            }
        }

        public bool UseContinuousSalePrinterEffective =>
            UseAllPrinters ? UseContinuousInvoicePrinter : UseContinuousSalePrinter;

        private bool _enablePrintSale;
        public bool EnablePrintSale
        {
            get => _enablePrintSale;
            set
            {
                if (_enablePrintSale != value)
                {
                    _enablePrintSale = value;
                    OnPropertyChanged(nameof(EnablePrintSale));
                }
            }
        }

        public ObservableCollection<string> InstalledPrintersSale { get; set; } = new();

        // ================== Global ==================
        private bool _useAllPrinters;
        public bool UseAllPrinters
        {
            get => _useAllPrinters;
            set
            {
                if (_useAllPrinters != value)
                {
                    _useAllPrinters = value;
                    OnPropertyChanged(nameof(UseAllPrinters));
                    OnPropertyChanged(nameof(IsEnabledOtherPrinters));   // 🔹 agregado
                    RaiseAllEffectiveThermalProperties();                // 🔹 agregado

                    if (value && !string.IsNullOrEmpty(InvoicePrinter))
                    {
                        RemitPrinter = InvoicePrinter;
                        BudgetPrinter = InvoicePrinter;
                        OrderPrinter = InvoicePrinter;
                        BarCodePrinter = InvoicePrinter;
                        TicketChangePrinter = InvoicePrinter;
                        SalePrinter = InvoicePrinter;
                    }
                }
            }
        }


        




        private void RaiseAllEffectiveThermalProperties()
        {
            OnPropertyChanged(nameof(UseContinuousRemitPrinterEffective));
            OnPropertyChanged(nameof(UseContinuousBudgetPrinterEffective));
            OnPropertyChanged(nameof(UseContinuousOrderPrinterEffective));
            OnPropertyChanged(nameof(UseContinuousBarCodePrinterEffective));
            OnPropertyChanged(nameof(UseContinuousTicketChangePrinterEffective));
            OnPropertyChanged(nameof(UseContinuousSalePrinterEffective));
            OnPropertyChanged(nameof(UseContinuousSalePrinter));
        }

        public void LoadInstalledPrinters()
        {
            InstalledPrintersInvoice.Clear();
            InstalledPrintersRemit.Clear();
            InstalledPrintersBudget.Clear();
            InstalledPrintersOrder.Clear();
            InstalledPrintersBarCode.Clear();
            InstalledPrintersTicketChange.Clear();
            InstalledPrintersSale.Clear();

            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                InstalledPrintersInvoice.Add(printer);
                InstalledPrintersRemit.Add(printer);
                InstalledPrintersBudget.Add(printer);
                InstalledPrintersOrder.Add(printer);
                InstalledPrintersBarCode.Add(printer);
                InstalledPrintersTicketChange.Add(printer);
                InstalledPrintersSale.Add(printer);
            }
        }

        [Display(Name = "Punto de venta")]
        public int SalePoint { get; set; }

        [Display(Name = "Nombre Pc")]
        public string? ComputerName { get; set; }
    }
}
