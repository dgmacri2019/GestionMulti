using GestionComercial.Contract.Interfaces;
using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using System.ServiceModel;

namespace GestionComercial.API.NamedPipe
{
    public class ReportClient
    {
        private readonly string _pipeAddress = "net.pipe://localhost/GestionComercial/ReportService";

        public async Task<ReportResponse> GenerateInvoicePdfAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura)
        {
            NetNamedPipeBinding binding = new();
            EndpointAddress endpoint = new(_pipeAddress);

            // ChannelFactory genera un proxy del servicio WCF
            var factory = new ChannelFactory<IReportService>(binding, endpoint);
            var proxy = factory.CreateChannel();

            try
            {
                // Llamada al servicio Windows Service
                var response = await proxy.GenerateInvoicePDFAsync(model, factura);
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