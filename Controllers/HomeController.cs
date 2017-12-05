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
        public IActionResult CreateUser([FromBody] UserAdd userAdd)
        {
            return Content(userAdd.Validate(userAdd));

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
        [HttpGet("ObtenerProducto")]
        public IActionResult ObtenerProducto(string nombreProduto)
        {

            return Content("");
        }
        [HttpGet("ObtenerProducto")]
        public IActionResult ObtenerProducto(int codigoBarras)
        {

            return Content("");
        }
        [HttpPost("NewProduct")]
        public IActionResult NewProduct([FromBody] Product product)
        {
            if (product.SaveProduct())
                return Content("Ok");

            return Content("Error");
        }
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserAdd user)
        {
            return Content(user.Login(user.Mail,user.Password));        
        }

    }
}
