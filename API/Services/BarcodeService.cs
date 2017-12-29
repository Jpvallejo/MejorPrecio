using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Http;
using ZXing;

namespace MejorPrecio3.API.Services
{
    public class BarcodeService
    {
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