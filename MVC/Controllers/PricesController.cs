using System.Linq;
using MejorPrecio3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MejorPrecio3.API;
using System;
using MejorPrecio3.API.Services;
using MejorPrecio3.Services;
using MejorPrecio3.MVC.Models;
using System.Drawing;
using System.Security.Claims;

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
            var model = new PriceViewModel();
            return View(model);
        }
        //[Authorize]
        [HttpPost]
        public ActionResult Create(PriceViewModel model)
        {
            var geocoder = new Geocoder();
            var latlong = geocoder.GetLatLong(model.location);
            if (!new CityService().IsInBsAs(new PointF((float)latlong.Item1, (float)latlong.Item2)))
            {
                model.location = String.Empty;
                ModelState.AddModelError("location", "La direccion es invalida");
                return View("Create", model);
            }
            var product = api.GetProductByName(model.selectedProduct);
            if (product.Name == null)
            {
                model.selectedProduct = String.Empty;
                ModelState.AddModelError("selectedProduct", "El producto no existe");
                return View("Create", model);
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
                return RedirectToAction("Index", "");

            }

            return View("Error");
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

            if (User.Identity.IsAuthenticated)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
                api.UpdateSearchHistory(userId, result.First().productName);
            }

            return View("Search", result);
        }
        [Route("searchByName")]
        [HttpGet("{name}")]
        public IActionResult SearchWithName(string name)
        {
            var result = api.SearchProductName(name).Select(Translate);
            if (User.Identity.IsAuthenticated)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
                api.UpdateSearchHistory(userId, name);
            }
            return View("search", result);
        }


        [Route("searchWithName")]
        [HttpGet("{name}")]
        public IActionResult SearchByName(string name)
        {
            var result = api.SearchProductName(name).Select(Translate);
            if (User.Identity.IsAuthenticated)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
                api.UpdateSearchHistory(userId, name);
            }
            return Json(result);
        }

        [Route("searchWithBarcode")]
        [HttpGet("{barcode}")]
        public IActionResult SearchByBarcode(string barcode)
        {

            var result = api.SearchProductBarcode(barcode).Select(Translate);

            if (User.Identity.IsAuthenticated)
            {
                var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Sid));
                api.UpdateSearchHistory(userId, result.First().productName);
            }

            return Json(result);
        }
    }
}


