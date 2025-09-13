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
            Accountings = 10,
            [Display(Name = "Listas de Precios")]
            PriceLists = 11,
            [Display(Name = "Usuarios")]
            Users = 12,
            [Display(Name = "Ordenes de Compra")]
            PurchaseOrders = 13,
            [Display(Name = "Back Up")]
            BackUps = 14,
            [Display(Name = "Presupuestos")]
            Budgets = 15,
            [Display(Name = "Pagos y Cobranzas")]
            Payments = 16,
            [Display(Name = "Base de Datos")]
            DataBases = 17,
            [Display(Name = "Permisos")]
            Permissions = 18,
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


        //Clase que cambio

        public enum ChangeClass
        {
            Category,
            State,
            DocumentType,
            IvaCondition,
            SaleCondition,
            Measure,
            Tax,
        }
    }
}
