using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using ZXing;

namespace mejor_precio_3.Services
{
    public class BarcodeService
    {
        private static byte[] ToByteArray(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }
        public string GetBarcode(IFormFile file)
        {
            string decoded = null;

            Bitmap barcode = null; ;
            
            var barcodeReader = new BarcodeReader();
            barcodeReader.Options.TryHarder = true;

            barcode = (Bitmap)Bitmap.FromStream(file.OpenReadStream());


            decoded = barcodeReader.Decode(barcode).Text;

            return decoded;
        }
    }
}