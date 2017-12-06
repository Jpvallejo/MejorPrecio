﻿using System;
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
    [Route("Home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

[Route("About")]
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
            if (product.SaveProduct())
                return Content("Ok");

            return Content("Error");
        }
      

    }
}
