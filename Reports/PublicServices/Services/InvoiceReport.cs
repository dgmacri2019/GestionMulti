using Reports.PublicServices.Interfaces;
using Reports.Responses;
using Reports.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.PublicServices.Services
{
    public class InvoiceReport : IInvoiceReport
    {

        public InvoiceReport()
        {

        }




        public async Task<ReportResponse> GenerateInvoicePDFAsync(List<InvoiceReportViewModel> model)
        {
            ReportResponse response = new ReportResponse { Success = false };
            try
            {


                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
