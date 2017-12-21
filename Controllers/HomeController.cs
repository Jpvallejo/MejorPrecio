using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using mejor_precio_3.Models;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace mejor_precio_3.Controllers
{
    public class HomeController : Controller
    {
        
        public IActionResult Index()
        {
            var persistence = new ProductPersistence();
            string json = JsonConvert.SerializeObject(persistence.GetAllProductNames());

//write string to file
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory+ @"\productlist.json", json);
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
       
       public IActionResult SearchWithName(string name)
       {
           var model = new Searched{value = name};
            TempData["name"] = name;
            return View(model);
       }

       public IActionResult SearchWithBarcode(string barcode)
       {
           var model = new Searched{value = barcode};
            TempData["name"] = barcode;
            return View(model);
       }
       
      

    }
}
