using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionComercial.Domain.Constant
{
    public class Enumeration
    { 
        // Tipo de documentos
        public enum DocumentType
        {
            [Display(Name = "CUIT")]
            CUIT = 80,

            [Display(Name = "CUIL")]
            CUIL = 86,

            [Display(Name = "DNI")]
            DNI = 96,

            [Display(Name = "Pasaporte")]
            Passport = 94,

            [Display(Name = "Otro")]
            Other = 99,

        }

        // tipo de Condición IVA 
        public enum TaxCondition
        {
            [Display(Name = "Seleccione el tipo de contrinuyente")]
            Select = 100,
            [Display(Name = "Responsable Inscripto")]
            IvaInscripto = 101,
            [Display(Name = "Consumidor Final")]
            ConsumidorFinal = 102,
            [Display(Name = "Exento")]
            IvaExento = 103,
            [Display(Name = "Responsable Monotributo")]
            Monotributo = 104
        }

        // Tipo de Condicion de venta
        public enum SaleCondition
        {
            [Display(Name = "Seleccione la Forma de Pago")]
            Select = 0,
            //[Display(Name = "Sin Cargo")]
            //SinCargo = 1,
            [Display(Name = "Cuenta Corriente")]
            CtaCte = 2,
            [Display(Name = "Cheque")]
            Cheque = 3,
            [Display(Name = "Efectivo Pesos")]
            EfectivoPeso = 4,
            [Display(Name = "Efectivo Dolar")]
            EfectivoDolar = 17,
            [Display(Name = "Efectivo Real")]
            EfectivoReal = 18,
            [Display(Name = "Efectivo Euro")]
            EfectivoEuro = 19,
            [Display(Name = "Efectivo Otro")]
            EfectivoOtro = 20,
            [Display(Name = "Transferencia Bancaria")]
            Deposito = 5,
            [Display(Name = "Tarjeta de Débito")]
            Debito = 6,
            [Display(Name = "Tarjeta de Crédito")]
            Credito = 7,
            [Display(Name = "Mercado Pago Transferencia")]
            MercadoPagoTransf = 8,
            [Display(Name = "Mercado Pago QR Crédito")]
            MercadoPagoQRCred = 9,
            [Display(Name = "Mercado Pago QR Débito")]
            MercadoPagoQRDebit = 10,
            [Display(Name = "Mercado Pago QR Dinero Cuenta")]
            MercadoPagoQRCta = 11,
            [Display(Name = "Mercado Pago Point Crédito")]
            MercadoPagoCredit = 12,
            [Display(Name = "Mercado Pago Point Débito")]
            MercadoPagoDebit = 13,
            [Display(Name = "Mercado Pago On-Line")]
            MercadoPagoOnLine = 14,
            [Display(Name = "Mercado Pago Link Pago")]
            MercadoPagoLink = 15,
            [Display(Name = "Mercado Pago Otro")]
            MercadoPagoOter = 16,
            [Display(Name = "Multiple Metodos de pago")]
            MultipleMethod = 21,

        }

        //Version del Sistema
        public enum SystemVersionType
        {
            [Display(Name = "Ventas")]
            Sale = 0X10,
            [Display(Name = "Compras y Ventas")]
            PurchaseSale = 0X11,
            [Display(Name = "Compras")]
            Purchase = 0X12,
            [Display(Name = "Administración")]
            Administration = 0X13,
            [Display(Name = "Compras, Ventas y Administración")]
            SalesPurchaseAdministration = 0X14,
            [Display(Name = "Contabilidad")]
            Accounting = 0X15,
            [Display(Name = "Compras, Ventas, Administración y Contabilidad")]
            SalesPurchaseAdministrationAccounting = 0X16
        }

        //Módulos del sistema
        public enum ModuleType
        {
            [Display(Name = "Clientes")]
            Clients = 1,
            [Display(Name = "Proveedores")]
            Providers = 2,
            [Display(Name = "Articulos")]
            Articles = 3,
            [Display(Name = "Compras")]
            Purchases = 4,
            [Display(Name = "Ventas")]
            Sales = 5,
            [Display(Name = "Bancos")]
            Banks = 6,
            [Display(Name = "Reportes")]
            Reports = 7,
            [Display(Name = "Parametros")]
            Parameters = 8,
            [Display(Name = "Configuraciones")]
            Settings = 9,
            [Display(Name = "Cuentas Contables")]
            Accounting = 10,
            [Display(Name = "Listas de Precios")]
            PriceList = 11,
            [Display(Name = "Usuarios")]
            Users = 12,
            [Display(Name = "Ordenes de Compra")]
            PurchaseOrders = 13,
            [Display(Name = "Back Up")]
            BackUp = 14,
            [Display(Name = "Presupuestos")]
            Budgets = 15,
            [Display(Name = "Pagos y Cobranzas")]
            Payments = 16,
            [Display(Name = "Base de Datos")]
            DataBase = 17,
            [Display(Name = "Permisos")]
            Permission = 18,
        }

    }
}
