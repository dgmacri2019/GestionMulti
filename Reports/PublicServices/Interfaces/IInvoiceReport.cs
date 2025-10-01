using Reports.Responses;
using Reports.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Reports.PublicServices.Interfaces
{
    public interface IInvoiceReport
    {
        Task<ReportResponse> GenerateInvoicePDFAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura);
    }
}
