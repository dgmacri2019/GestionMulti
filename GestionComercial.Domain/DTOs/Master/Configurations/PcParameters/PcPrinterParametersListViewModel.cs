using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.DTOs.Master.Configurations.PcParameters
{
    public class PcPrinterParametersListViewModel
    {
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

        [Display(Name = "Impresora de Facturas")]
        public string? InvoicePrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Facturas")]
        public int MaxWidthInvoicePrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Facturas")]
        public bool UseContinuousInvoicePrinter { get; set; }




        [Display(Name = "Impresora de Remitos")]
        public string? RemitPrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Remitos")]
        public int MaxWidthRemitPrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Remitos")]
        public bool UseContinuousRemitPrinter { get; set; }




        [Display(Name = "Impresora de Presupuestos")]
        public string? BudgetPrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Presupuestos")]
        public int MaxWidthBudgetPrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Presupuestos")]
        public bool UseContinuousBudgetPrinter { get; set; }




        [Display(Name = "Impresora de Pedidos")]
        public string? OrderPrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Pedidos")]
        public int MaxWidthOrderPrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Pedidos")]
        public bool UseContinuousOrderPrinter { get; set; }




        [Display(Name = "Impresora de Códigos de Barra")]
        public string? BarCodePrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Códigos de Barra")]
        public int MaxWidthBarCodePrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Códigos de Barra")]
        public bool UseContinuousBarCodePrinter { get; set; }




        [Display(Name = "Impresora de Ticket de Cambio")]
        public string? TicketChangePrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Ticket de Cambio")]
        public int MaxWidthTicketChangePrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Ticket de Cambio")]
        public bool UseContinuousTicketChangePrinter { get; set; }




        [Display(Name = "Impresora de Ventas")]
        public string? SalePrinter { get; set; }

        [Display(Name = "Ancho Maximo Impresora de Ventas")]
        public int MaxWidthSalePrinter { get; set; }

        [Display(Name = "Usar Papel Continuo Impresora de Ventas")]
        public bool UseContinuousSalePrinter { get; set; }




        [Display(Name = "Usar la misma impresora")]
        public bool UseAllPrinters { get; set; }
        public bool EnablePrintInvoice { get; set; }
        public bool EnablePrintBudget { get; set; }
        public bool EnablePrintOrder { get; set; }
        public bool EnablePrintRemit { get; set; }
        public bool EnablePrintBarCode { get; set; }
        public bool EnablePrintTicketChange { get; set; }
        public bool EnablePrintSale { get; set; }


        [Display(Name = "Punto de venta")]
        public string? SalePoint { get; set; }

        [Display(Name = "Nombre Pc")]
        public string? ComputerName { get; set; }
    }
}
