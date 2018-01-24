using System.Linq;
using MejorPrecio3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MejorPrecio3.API;
using System;
using MejorPrecio3.API.Services;
using MejorPrecio3.Services;
using System.Security.Claims;
using System.Drawing;
using MejorPrecio3.RESTApi.Models;

namespace MejorPrecio3.Controllers
{
    [Route("Prices")]
    public class PricesController : Controller
    {
        private SearchBestPrice api = new SearchBestPrice();

        public IActionResult Index()
        {
            try
            {
                return StatusCode(200, api.GetAllPrices());
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [Route("List")]
        public JsonResult List(string Prefix)
        {
            var model = new PriceViewModel();
            var prodlist = api.GetSimilarNames(Prefix)
                .Select(s => new { Name = s });

            return Json (prodlist);
        }

        //[Authorize]
        [Route("Create")]
        [HttpPost]
        public ActionResult Create([FromBody]PriceViewModel model)
        {
            var geocoder = new Geocoder();
            var latlong = geocoder.GetLatLong(model.location);
            if (!new CityService().IsInBsAs(new PointF((float)latlong.Item1, (float)latlong.Item2)))
            {
                return StatusCode(400, "La direccion debe ser dentro de CABA");
            }
            var product = api.GetProductByName(model.selectedProduct);
            if (product.Name == null)
                {
                    return StatusCode(400,"El producto no existe");
                }
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
            return StatusCode(200, "Product deleted successfully");
        }

        [Route("searchByBarcode")]
        [HttpGet("{barcode}")]
        public IActionResult SearchWithBarcode(string barcode)
        {
            try 
            {
                var result = api.SearchProductBarcode(barcode);
                if (User.Identity.IsAuthenticated)
                {
                    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
                    api.UpdateSearchHistory(userId, result.First().product.Name);
                }

                return StatusCode(200, result);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }
        [Route("searchByName/{name}")]
        [HttpGet()]
        public IActionResult SearchWithName(string name)
        {
            try
            {
                var result = api.SearchProductName(name);
                if (User.Identity.IsAuthenticated)
                {
                    var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
                    api.UpdateSearchHistory(userId, name);
                }
                return StatusCode(200, Json(result));
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);
            }
        }

        [Route("BarcodeUploading")]
        [HttpPost]
        public IActionResult BarcodeUploading(IFormFile file)
        {
            var barcodeService = new BarcodeService();
            var barcode = barcodeService.GetBarcode(file);

            return RedirectToAction("SearchWithBarcode", new { barcode = barcode });
        }
    }
}