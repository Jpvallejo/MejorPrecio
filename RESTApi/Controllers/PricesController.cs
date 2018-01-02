using System.Linq;
using MejorPrecio3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MejorPrecio3.API;
using System;
using MejorPrecio3.API.Services;
using MejorPrecio3.Services;

namespace MejorPrecio3.Controllers
{
    [Route("Prices")]
    public class PricesController : Controller
    {
        private SearchBestPrice api = new SearchBestPrice();

        public IActionResult Index()
        {
            return Json(api.GetAllPrices());
        }

        [Route("List")]
        public JsonResult List(string Prefix)
        {
            var model = new ProductViewModel();
            var prodlist = api.GetSimilarNames(Prefix)
                .Select(s => new { Name = s });

            return Json(prodlist);
        }

        //[Authorize]
        [Route("")]
        [HttpPost]
        public ActionResult Create([FromBody]ProductViewModel model)
        {
            var geocoder = new Geocoder();
            var latlong = geocoder.GetLatLong(model.location);
            var product = api.GetProductByName(model.selectedProduct);
            var price = new Price()
            {
                price = model.price,
                Id = Guid.Empty,
                latitude = latlong.Item1,
                longitude = latlong.Item2

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

            var result = api.SearchProductBarcode(barcode);

            return Json(result);
        }
        [Route("searchByName")]
        [HttpGet("{name}")]
        public IActionResult SearchWithName(string name)
        {
            var result = api.SearchProductName(name);
            return Json(result);
        }

        [Route("BarcodeUploading")]
        [HttpPost]
        public IActionResult BarcodeUploading(IFormFile file)
        {
            var barcodeService = new BarcodeService();
            var barcode = barcodeService.GetBarcode(file);

            return Content(barcode);
        }


    }
}