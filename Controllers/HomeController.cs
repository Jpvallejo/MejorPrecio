using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejor_precio_3.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace mejor_precio_3.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            User user1 = new User();
            string result = user1.Add(user);
            if (result == "OK")
            {
                //persistencia
                return Content("OK");
            }
            else
            {
                return Content(result);
            }
        }
        [HttpPut("ModificarContraseña")]
        public IActionResult ModificarContraseña(string Email, string PassAnt)
        {

            return Content("");
        }
        [HttpPost("ObtenerHistorial")]
        public IActionResult ObtenerHistorial(string Email)
        {

            return Content("");
        }
        [HttpPut("ActualizarHistorial")]
        public IActionResult ActualizarHistorial(string Email, string ProductoBuscado)
        {

            return Content("");
        }
        [HttpDelete("RemoveProduct")]
        public IActionResult RemoveProduct([FromBody] Product product)
        {
            product.Delete();
            return Content("Product deleted successfully");
        }


        [HttpGet("{productName}", Name ="SearchProduct")]
        public IActionResult SearchProduct(string productName)
        {
            SearchBar searchbar = new SearchBar();
            List<Product> productList = searchbar.searchProduct(productName);
            
            if (productList.Count==0)
            {
                return Content("No se encontro ningun producto");
            }
            else {
                return Json(productList);
            }
        }


        [HttpGet("{productBarCode}", Name = "barCode")]
        public IActionResult SearchProduct(int barCode)
        {
            SearchBar searchbar = new SearchBar();
            List<Product> productList = searchbar.searchProduct(barCode);

            if (productList.Count == 0)
            {
                return Content("No se encontro ningun producto");
            }
            else
            {
                return Json(productList);
            }
        }

        [HttpPost("NewProduct")]
        public IActionResult NewProduct([FromBody] Product product)
        {
            if(product.SaveProduct())
                return Content("Ok");

            return Content("Error");
        }

    }
}
