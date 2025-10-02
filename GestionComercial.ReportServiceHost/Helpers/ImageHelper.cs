using GestionComercial.Contract.Responses;
using GestionComercial.Contract.ViewModels;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Newtonsoft.Json;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace GestionComercial.ReportServiceHost.Helpers
{
    internal class ImageHelper
    {
        public static ReportResponse GenerateQRCodeToByteArray(QrDataViewModel data)
        {
            try
            {

                QrEncoder qrEncoder = new QrEncoder(ErrorCorrectionLevel.H);
                QrCode qrCode = qrEncoder.Encode(string.Format("{0}{1}", "https://www.afip.gob.ar/fe/qr/?p=", GenerateCode(data)));
                byte[] result;
                GraphicsRenderer renderer = new GraphicsRenderer(new FixedModuleSize(1, QuietZoneModules.Two), Brushes.Black, Brushes.White);
                using (var stream = new MemoryStream())
                {
                    renderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, stream);

                    result = stream.ToArray();
                }


                return new ReportResponse
                {
                    Success = true,
                    Bytes = result,
                };
            }
            catch (Exception ex)
            {
                return new ReportResponse
                {
                    Success = false,
                    Message = ex.Message,
                };
            }
        }


















        private static string GenerateCode(QrDataViewModel model)
        {
            try
            {
                var json = JsonConvert.SerializeObject(model);
                string result = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

                return result;
            }
            catch (Exception)
            {
                return null;
            }


        }

    }
}
