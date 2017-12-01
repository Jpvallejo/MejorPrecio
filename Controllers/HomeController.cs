using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        [HttpPost("CrearUsuario")]
        public IActionResult CrearUsuario(string Nombre, string Email, string Pass, string sexo, int edad)
        {

            return Content("");
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
        [HttpDelete("EliminarProducto")]
        public IActionResult EliminarProducto(string Email)
        {

            return Content("");
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
           [HttpGet("CargarProducto")]
        public IActionResult CargarProducto(string Nombre, string Precio,string Direccion)
        {

            return Content("");
        }
         public IActionResult ObtenerProductos(string NombreProducto)
        {

            return Content("");
        }

    }
}
