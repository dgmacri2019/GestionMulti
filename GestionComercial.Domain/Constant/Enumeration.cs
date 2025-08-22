using System.ComponentModel.DataAnnotations;

namespace GestionComercial.Domain.Constant
{
    public class Enumeration
    {
        /*
        // Tipo de documentos
        public enum DocumentType
        {
            [Display(Name = "Seleccione el tipo de documento")]
            Seleccione = 0,

            [Display(Name = "CUIT")]
            CUIT = 80,

            [Display(Name = "CUIL")]
            CUIL = 86,

            [Display(Name = "DNI")]
            DNI = 96,

            [Display(Name = "Pasaporte")]
            Pasaporte = 94,

            [Display(Name = "Otro")]
            Otro = 99,

        }

        // tipo de Condición IVA 
        public enum TaxCondition
        {
            [Display(Name = "Seleccione el tipo de contrinuyente")]
            Seleccione = 0,
            [Display(Name = "Responsable Inscripto")]
            Iva_Inscripto = 101,
            [Display(Name = "Consumidor Final")]
            Consumidor_Final = 102,
            [Display(Name = "Exento")]
            Iva_Exento = 103,
            [Display(Name = "Responsable Monotributo")]
            Monotributo = 104
        }


        
        // Tipo de Condicion de venta
        public enum SaleCondition
        {
            [Display(Name = "Seleccione la Forma de Pago")]
            Seleccione = 0,
            //[Display(Name = "Sin Cargo")]
            //SinCargo = 1,
            [Display(Name = "Cuenta Corriente")]
            Cuenta_Corriente = 2,
            [Display(Name = "Cheque")]
            Cheque = 3,
            [Display(Name = "Efectivo Pesos")]
            Efectivo_Peso = 4,
            [Display(Name = "Efectivo Dolar")]
            Efectivo_Dolar = 17,
            [Display(Name = "Efectivo Real")]
            Efectivo_Real = 18,
            [Display(Name = "Efectivo Euro")]
            Efectivo_Euro = 19,
            [Display(Name = "Efectivo Otro")]
            Efectivo_Otro = 20,
            [Display(Name = "Transferencia Bancaria")]
            Deposito = 5,
            [Display(Name = "Tarjeta de Débito")]
            Debito = 6,
            [Display(Name = "Tarjeta de Crédito")]
            Credito = 7,
            [Display(Name = "Mercado Pago Transferencia")]
            Mercado_Pago_Transfencia = 8,
            [Display(Name = "Mercado Pago QR Crédito")]
            MercadoPago_QR_Credito = 9,
            [Display(Name = "Mercado Pago QR Débito")]
            MercadoPago_QR_Debito = 10,
            [Display(Name = "Mercado Pago QR Dinero Cuenta")]
            MercadoPago_QR_Dinero_Cuenta = 11,
            [Display(Name = "Mercado Pago Point Crédito")]
            MercadoPago_Credito = 12,
            [Display(Name = "Mercado Pago Point Débito")]
            MercadoPago_Debito = 13,
            [Display(Name = "Mercado Pago On-Line")]
            MercadoPago_OnLine = 14,
            [Display(Name = "Mercado Pago Link Pago")]
            MercadoPago_Link_Pago = 15,
            [Display(Name = "Mercado Pago Otro")]
            MercadoPago_Otro = 16,
            [Display(Name = "Multiple Metodos de pago")]
            Multiples_Metodos = 21,

        }

        */

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
            [Display(Name = "Lista de Precios")]
            PriceLists = 19,
        }


        // Tipo de factura a emitir
        public enum VaucherType
        {

            [Display(Name = "Factura A")]
            FacturaA = 1,

            [Display(Name = "Nota de Débito A")]
            NotaDebitoA = 2,

            [Display(Name = "Nota de Crédito A")]
            NotaCridtoA = 3,

            [Display(Name = "Recibo A")]
            ReciboA = 4,

            [Display(Name = "Nota de Venta al Contado A")]
            NotaContadoA = 5,

            [Display(Name = "Factura B")]
            FacturaB = 6,

            [Display(Name = "Nota de Débito B")]
            NotaDebitoB = 7,

            [Display(Name = "Nota de Crédito B")]
            NotaCridtoB = 8,

            [Display(Name = "Recibo B")]
            ReciboB = 9,

            [Display(Name = "Nota de Venta al Contado B")]
            NotaContadoB = 10,

            [Display(Name = "Factura C")]
            FacturaC = 11,

            [Display(Name = "Nota de Débito C")]
            NotaDebitoC = 12,

            [Display(Name = "Nota de Crédito C")]
            NotaCridtoC = 13,

            [Display(Name = "Recibo C")]
            ReciboC = 15,

            [Display(Name = "Factura M")]
            FacturaM = 51,

            [Display(Name = "Nota de Débito M")]
            NotaDebitoM = 52,

            [Display(Name = "Nota de Crédito M")]
            NotaCridtoM = 53,

            [Display(Name = "Recibo M")]
            ReciboM = 54,

            [Display(Name = "Presupuesto")]
            Presupuesto = 999,
        }


        //Tipos de cambios

        public enum ChangeType
        {
            Created, 
            Updated, 
            Deleted
        }

    }
}
