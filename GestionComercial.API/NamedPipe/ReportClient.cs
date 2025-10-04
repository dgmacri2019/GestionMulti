using GestionComercial.Contract.Interfaces;
using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using System.ServiceModel;

namespace GestionComercial.API.NamedPipe
{
    public class ReportClient
    {
        // Dirección local por NamedPipe
        private readonly string _pipeAddress = "net.pipe://localhost/GestionComercial/ReportService";

        // Dirección por TCP en la red
        private readonly string _tcpAddress = "net.tcp://192.168.254.150:9000/GestionComercial/ReportService";

        public async Task<ReportResponse> GenerateInvoicePdfAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura)
        {
            // Configuración del binding para TCP
            var binding = new NetTcpBinding
            {
                MaxReceivedMessageSize = 10_000_000,
                Security = { Mode = SecurityMode.None } // 🔑 Desactivar autenticación
            };

            // También podrías usar NamedPipe si solo fuera local
            // var binding = new NetNamedPipeBinding
            // {
            //     MaxReceivedMessageSize = 10_000_000,
            //     Security = { Mode = NetNamedPipeSecurityMode.None }
            // };
            // var endpoint = new EndpointAddress(_pipeAddress);

            var endpoint = new EndpointAddress(_tcpAddress);

            // ChannelFactory genera un proxy del servicio WCF
            var factory = new ChannelFactory<IReportService>(binding, endpoint);
            var proxy = factory.CreateChannel();

            try
            {
                // Llamada al servicio Windows Service
                ReportResponse response = await proxy.GenerateInvoicePDFAsync(model, factura);

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
                // Cerramos el proxy correctamente
                if (proxy is IClientChannel channel)
                {
                    try { channel.Close(); } catch { channel.Abort(); }
                }
            }
        }

        public async Task<ReportResponse> GenerateSalePdfAsync(List<InvoiceReportViewModel> model, FacturaViewModel factura)
        {
            // Configuración del binding para TCP
            var binding = new NetTcpBinding
            {
                MaxReceivedMessageSize = 10_000_000,
                Security = { Mode = SecurityMode.None } // 🔑 Desactivar autenticación
            };

            // También podrías usar NamedPipe si solo fuera local
            // var binding = new NetNamedPipeBinding
            // {
            //     MaxReceivedMessageSize = 10_000_000,
            //     Security = { Mode = NetNamedPipeSecurityMode.None }
            // };
            // var endpoint = new EndpointAddress(_pipeAddress);

            var endpoint = new EndpointAddress(_tcpAddress);

            // ChannelFactory genera un proxy del servicio WCF
            var factory = new ChannelFactory<IReportService>(binding, endpoint);
            var proxy = factory.CreateChannel();

            try
            {
                // Llamada al servicio Windows Service
                ReportResponse response = await proxy.GenerateSalePDFAsync(model, factura);

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
                // Cerramos el proxy correctamente
                if (proxy is IClientChannel channel)
                {
                    try { channel.Close(); } catch { channel.Abort(); }
                }
            }
        }
    }
}
