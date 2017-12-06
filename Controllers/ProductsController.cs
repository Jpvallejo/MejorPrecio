using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using mejor_precio_3.Models;
using Microsoft.AspNetCore.Mvc;

namespace mejor_precio_3.Controllers
{
    [Route("Products")]
    public class ProductsController : Controller
    {
        private List<Product> mockProducts = new List<Product>();
        private SearchBar searchBar = new SearchBar();

        public IActionResult Index()
        {
            return View();
        }

        [Route("CreateProduct")]
        [HttpPost]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            mockProducts.Add(product);
            mockProducts.Sort((x, y) => x.Price.CompareTo(y.Price));
            return Content("Product added correctly");
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct([FromBody] Product product)
        {
            mockProducts.Remove(product);
            return Content("Product deleted successfully");
        }


        [HttpGet("{barcode}", Name = "SearchWithBarcode")]
        public IActionResult SearchWithBarcode(int barcode)
        {
            List<Product> result = searchBar.SearchProductBarcode(mockProducts, barcode);
            return Json(result);
        }

        [HttpGet("{name}", Name = "SearchWithName")]
        public IActionResult SearchWithName(string name)
        {
            List<Product> result = searchBar.SearchProductName(mockProducts, name);
            return Json(result);
        }


    }
}