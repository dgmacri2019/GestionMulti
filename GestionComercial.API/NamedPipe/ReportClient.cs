using GestionComercial.Contract.Interfaces;
using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using System.ServiceModel;

namespace GestionComercial.API.NamedPipe
{
    public class ReportClient
    {
        private readonly string _pipeAddress = "net.pipe://localhost/GestionComercial/ReportService";
        //private readonly string _pipeAddress = "net.pipe://localhost/GestionComercial";

        public async Task<ReportResponse> GenerateInvoicePdfAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura)
        {
            NetNamedPipeBinding binding = new()
            {
                MaxReceivedMessageSize = 10_000_000
            };
            EndpointAddress endpoint = new(_pipeAddress);

            // ChannelFactory genera un proxy del servicio WCF
            var factory = new ChannelFactory<IReportService>(binding, endpoint);
            var proxy = factory.CreateChannel();

            try
            {
                // Llamada al servicio Windows Service
                ReportResponse response = await proxy.GenerateInvoicePDFAsync(model, factura);
                if (response.Success)
                { }
                else
                { }
                return response;
            }
            catch (Exception ex)
            {
                return new ReportResponse
                {
                    Success = false,
                    Message = $"Error al llamar al servicio de reporte: {ex.Message}"
                };
            }
            finally
            {
                // Cerramos el proxy
                if (proxy is IClientChannel channel)
                {
                    try { channel.Close(); } catch { channel.Abort(); }
                }
            }
        }
    }
}