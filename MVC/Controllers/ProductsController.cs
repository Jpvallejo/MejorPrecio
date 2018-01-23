using MejorPrecio3.Models;
using Microsoft.AspNetCore.Mvc;
using MejorPrecio3.API;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MejorPrecio3.API.Services;

namespace MejorPrecio3.MVC.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {

        private SearchBestPrice api = new SearchBestPrice();

    [Authorize(Roles ="admin")]
        [HttpGet]
        public IActionResult Index()
        {
            return View(api.GetAllProducts());
        }

        [Authorize]
        [HttpGet("Create")]
        
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [Route("")]
        [HttpPost]
        public ActionResult Create(Product product)
        {
            product.Id = Guid.Empty;
            if (api.SaveProduct(product))
            {
                return StatusCode(204);

            }

            return StatusCode(500);
        }
        [Authorize(Roles = "admin")]
        [Route("{id}")]
        public IActionResult Delete(Guid id)
        {
            // product.product = prod;
            api.DeleteProduct(id);
            return Content("Product deleted successfully");
        }


        [HttpPost("GetBarcode")]
        public IActionResult GetBarcode(IFormFile image)
        {
            if (HttpContext.Request.Form.Files.Count > 0)
        {
            image = HttpContext.Request.Form.Files.GetFile("file");
        }
            var barcodeService = new BarcodeService();
            var barcode = barcodeService.GetBarcode(image);

            return Content(barcode);
        }

    }
}