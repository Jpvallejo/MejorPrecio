using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using mejor_precio_3.Models;

namespace mejor_precio_3.Controllers
{
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
