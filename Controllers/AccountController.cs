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
{[Route("Account")]
    public class AccountController : Controller
    {
        
        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserAdd userAdd)
        {
            return Content(userAdd.Validate(userAdd));

        }
        [HttpPut("ModificarContraseña")]
        public IActionResult ModificarContraseña(string Email, string PassAnt,string newPass)
        {

            return Content("");
        }
        public IActionResult Login([FromBody] UserAdd user)
        {
            return Content("");
        }
        public IActionResult LogOff([FromBody] UserAdd user)
        {
            return Content("");
        }
        public IActionResult GetHistory(string Email)
        {

            return Content("");
        }
        [HttpPut("ActualizarHistorial")]
        public IActionResult UpdateHistory(string Email, string ProductoBuscado)
        {

            return Content("");
        }

    }
}
