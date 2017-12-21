using System.Collections.Generic;
using System.Linq;
using mejor_precio_3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.AspNetCore.Authorization;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace mejor_precio_3.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private ProductPersistence persistence = new ProductPersistence();
        private SearchBar searchBar = new SearchBar();

        public IActionResult Index()
        {
            return View();
        }

        [Route("Create")]
        public ActionResult Create()
        {
            var model = new ProductViewModel();
            model.ProductsNames = persistence.GetAllProductNames();
            ViewBag.lista = model.ProductsNames;
            return View(model);
        }

        [Route("List")]
        public JsonResult List(string Prefix)
        {
            var model = new ProductViewModel();
            var prodlist = persistence.GetProductAutoComplete(Prefix)
                .Select(s => new { Name = s });

            return Json(prodlist);
        }

        //[Authorize]
        [Route("Create")]
        [HttpPost]
        public ActionResult Create(ProductViewModel model)
        {
            var product = new Price ();
            if(product.SaveProduct(model))
            {
                return RedirectToAction("Index", "");//Content("Product added correctly");

            }

            return Content("Error");
        }
        [Authorize(Roles = "admin")]
        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct([Bind("Name,Barcode,Brand")]Product prod, [Bind("price,location")] Price product)
        {
            // product.product = prod;
            byte[] objectBytes;
            List<Price> mockProducts;
            if (HttpContext.Session.TryGetValue("List", out objectBytes))
            {
                //var objectBytes = HttpContext.Session.Get("List");
                var chargingStream = new MemoryStream();
                var binFormatterGetting = new BinaryFormatter();

                // Where 'objectBytes' is your byte array.
                chargingStream.Write(objectBytes, 0, objectBytes.Length);
                chargingStream.Position = 0;

                mockProducts = binFormatterGetting.Deserialize(chargingStream) as List<Price>;
                mockProducts.Remove(product);
            }
            return Content("Product deleted successfully");
        }

        [Route("searchByBarcode")]
        [HttpGet("{barcode}")]
        public IActionResult SearchWithBarcode(string barcode)
        {
            
            var result = searchBar.SearchProductBarcode(barcode);

            return Json(result);
        }
        [Route("searchByName")]
        [HttpGet("{name}")]
        public IActionResult SearchWithName(string name)
        {
            var result = searchBar.SearchProductName(name);
            return Json(result);
        }

        [HttpPost]
        public IActionResult BarcodeUploading(IFormFile file)
        {
            var barcodeService = new BarcodeService();
            var barcode = barcodeService.GetBarcode(file);

            return RedirectToAction("SearchWithBarcode","",new{barcode =barcode}); 
        }


    }
}