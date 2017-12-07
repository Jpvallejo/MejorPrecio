using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mejor_precio_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace mejor_precio_3.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private SearchBar searchBar = new SearchBar();

        public IActionResult Index()
        {
            return View();
        }

        [Route("CreateProduct")]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            byte[] objectBytes;
            List<Product> mockProducts;
            if (HttpContext.Session.TryGetValue("List", out objectBytes))
            {
                //var objectBytes = HttpContext.Session.Get("List");
                var chargingStream = new MemoryStream();
                var binFormatterGetting = new BinaryFormatter();

                // Where 'objectBytes' is your byte array.
                chargingStream.Write(objectBytes, 0, objectBytes.Length);
                chargingStream.Position = 0;

                mockProducts = binFormatterGetting.Deserialize(chargingStream) as List<Product>;
            }
            else
            {
                mockProducts = new List<Product>();
            }
            if(mockProducts.Exists(x => x.Location == product.Location)){
                mockProducts.Remove(mockProducts.Find(x => x.Location == product.Location));
            }
            mockProducts.Add(product);
            mockProducts.Sort((x, y) => x.Price.CompareTo(y.Price));

            var mStream = new MemoryStream();
            var binFormatter = new BinaryFormatter();
            binFormatter.Serialize(mStream, mockProducts);
            var listOfBytes = mStream.ToArray();
            HttpContext.Session.Set("List", listOfBytes);
            return Content("Product added correctly");
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct([FromBody] Product product)
        {
            byte[] objectBytes;
            List<Product> mockProducts;
            if (HttpContext.Session.TryGetValue("List", out objectBytes))
            {
                //var objectBytes = HttpContext.Session.Get("List");
                var chargingStream = new MemoryStream();
                var binFormatterGetting = new BinaryFormatter();

                // Where 'objectBytes' is your byte array.
                chargingStream.Write(objectBytes, 0, objectBytes.Length);
                chargingStream.Position = 0;

                mockProducts = binFormatterGetting.Deserialize(chargingStream) as List<Product>;
                mockProducts.Remove(product);
            }
            return Content("Product deleted successfully");
        }

        [Route("search/byBarcode")]
        [HttpGet("{barcode}")]
        public IActionResult SearchWithBarcode(int barcode)
        {
            List<Product> result = null;
            byte[] objectBytes;
            List<Product> mockProducts;
            if (HttpContext.Session.TryGetValue("List", out objectBytes))
            {
                //var objectBytes = HttpContext.Session.Get("List");
                var chargingStream = new MemoryStream();
                var binFormatterGetting = new BinaryFormatter();

                // Where 'objectBytes' is your byte array.
                chargingStream.Write(objectBytes, 0, objectBytes.Length);
                chargingStream.Position = 0;

                mockProducts = binFormatterGetting.Deserialize(chargingStream) as List<Product>;
                result = searchBar.SearchProductBarcode(mockProducts, barcode);
            }
            return Json(result);
        }
        [Route("search/byName")]
        [HttpGet("{name}")]
        public IActionResult SearchWithName(string name)
        {
            List<Product> result = null;
            byte[] objectBytes;
            List<Product> mockProducts;
            if (HttpContext.Session.TryGetValue("List", out objectBytes))
            {
                //var objectBytes = HttpContext.Session.Get("List");
                var chargingStream = new MemoryStream();
                var binFormatterGetting = new BinaryFormatter();

                // Where 'objectBytes' is your byte array.
                chargingStream.Write(objectBytes, 0, objectBytes.Length);
                chargingStream.Position = 0;

                mockProducts = binFormatterGetting.Deserialize(chargingStream) as List<Product>;
                result = searchBar.SearchProductName(mockProducts, name);
            }
            return Json(result);
        }


    }
}