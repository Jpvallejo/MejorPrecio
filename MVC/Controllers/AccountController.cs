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

        [HttpPost]
        public IActionResult Post([FromBody] UserAdd userAdd)
        {

            User user = new User()
            {
                Age = userAdd.Age,
                Verified = 0,
                Role = "user",
                Password = userAdd.Password,
                Gender = userAdd.Gender,
                Mail = userAdd.Mail,
                Name = userAdd.Name
            };
            bool exists = api.Exist(user.Mail);
            if (!exists)
            {
                try
                {
                    api.CreateUser(user);
                }
                catch(Exception e)
                {
                    return StatusCode(412,e.Message);
                }
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

        [Route("GetHistory")]
        [HttpGet("{userId}")]
        public IActionResult GetHistory(Guid userId)
        {
            try{
                var searchHistroy = Json(api.GetSearchHistory(userId));
            }

            catch (Exception e){
                return StatusCode(412,e.Message);
            }
            return StatusCode(200, searchHistory);
        }

        [HttpPatch("ActualizarHistorial")]
        public IActionResult UpdateHistory([FromBody]User user)
        {
            try{
                api.UpdateSearchHistory(user);
            }

            catch (Exception e){
                return StatusCode(400, e.Message);
            }
            return StatusCode(200);
        }

    }
}
