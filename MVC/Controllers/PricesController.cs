using System.Linq;
using MejorPrecio3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MejorPrecio3.API;
using System;
using MejorPrecio3.API.Services;
using MejorPrecio3.Services;
using MejorPrecio3.MVC.Models;

namespace MejorPrecio3.MVC.Controllers
{
    [Route("Prices")]
    public class PricesController : Controller
    {
        private SearchBestPrice api = new SearchBestPrice();


        private SearchViewModel Translate(Price p)
        {
            return new SearchViewModel()
            {
                price = p.price,
                productName = p.product.Name,
                productBrand = p.product.Brand,
                productBarcode = p.product.Barcode,
                latitude = p.latitude,
                longitude = p.longitude,
                adress = new Geocoder().GetAdress(p.latitude, p.longitude)
            };
        }

        public IActionResult Index()
        {
            return View(api.GetAllPrices());
        }
        
        [Route("List")]
        public JsonResult List(string Prefix)
        {
            var prodlist = api.GetSimilarNames(Prefix)
                .Select(s => new { Name = s });

            return Json(prodlist);
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            PriceViewModel model= null;
            return View(model);
        }
        //[Authorize]
        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]PriceViewModel model)
        {
            var geocoder = new Geocoder();
            var latlong = geocoder.GetLatLong(model.location);
            var product = api.GetProductByName(model.selectedProduct);
            var price = new Price()
            {
                price = model.price,
                Id = Guid.Empty,
                latitude = latlong.Item1,
                longitude = latlong.Item2,
                product = product

            };
            if (api.SavePrice(price))
            {
                return StatusCode(204);

            }

            return StatusCode(500);
        }
        // [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            // product.product = prod;
            api.DeletePrice(id);
            return Content("Product deleted successfully");
        }

        [Route("searchByBarcode")]
        [HttpGet("{barcode}")]
        public IActionResult SearchWithBarcode(string barcode)
        {

            var result = api.SearchProductBarcode(barcode).Select(Translate);

            return View("Search", result);
        }
        [Route("searchByName")]
        [HttpGet("{name}")]
        public IActionResult SearchWithName(string name)
        {
            var result = api.SearchProductName(name).Select(Translate);
            return View("search", result);
        }

        [Route("BarcodeUploading")]
        [HttpPost]
        public IActionResult BarcodeUploading(IFormFile file)
        {
            var barcodeService = new BarcodeService();
            var barcode = barcodeService.GetBarcode(file);

            return RedirectToAction("SearchWithBarcode", "Prices", new { barcode = barcode });
        }


    }
}