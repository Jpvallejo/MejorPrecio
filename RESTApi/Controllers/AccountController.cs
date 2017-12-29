using System;
using Microsoft.AspNetCore.Mvc;
using MejorPrecio3.Models;
using MejorPrecio3.API;
using System.Text.RegularExpressions;

namespace MejorPrecio3.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {

        SearchBestPrice api = new SearchBestPrice();

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserAdd userAdd)
        {
            try
            {
                var pass = userAdd.Password;
                Regex pat = new Regex(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$");

                if (!pat.IsMatch(userAdd.Password))
                    throw new Exception("Error en la contrasena +8, Mayus,Minus,numero");
                Regex regex = new Regex(@"(\w+)@(\w+)\.(\w+)");
                if (!regex.IsMatch(userAdd.Mail))
                    throw new Exception("Error en el mail");
            }
            catch (Exception e)
            {
                return Content(e.Message);
            }

            byte[] data = System.Text.Encoding.ASCII.GetBytes(userAdd.Password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            User user = new User()
            {
                Age = userAdd.Age,
                Verified = 0,
                Role = "user",
                Password = System.Text.Encoding.ASCII.GetString(data),
                Gender = userAdd.Gender,
                Mail = userAdd.Mail,
                Name = userAdd.Name
            };
        bool exists = api.Exist(user.Mail);
                if (!exists){
                    api.CreateUser(user);
                    return StatusCode(204);
    }
                else
                {
                    return Content("El usuario ya existe");
}



            }
        [HttpPut("ModificarContraseña")]
public IActionResult ModificarContraseña(string Email, string PassAnt, string newPass)
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
