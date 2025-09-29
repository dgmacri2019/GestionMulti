using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GestionComercial.Desktop.Helpers
{
    internal class ImageHelper
    {
        public static BitmapImage ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0)
                return null;

            using (var ms = new MemoryStream(imageData))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // importante para cerrar el stream
                image.StreamSource = ms;
                image.EndInit();
                image.Freeze(); // para usar en binding sin problemas de hilos
                return image;
            }
        }
    }
}
