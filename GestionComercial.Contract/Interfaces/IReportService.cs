using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GestionComercial.Contract.Interfaces
{
    [ServiceContract]
    public interface IReportService
    {
        [OperationContract]
        Task<ReportResponse> GenerateInvoicePDFAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura);
        
        [OperationContract] 
        Task<ReportResponse> GenerateSalePDFAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura);
    }
}
