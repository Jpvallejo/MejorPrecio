using System;
using Microsoft.AspNetCore.Mvc;
using MejorPrecio3.Models;
using MejorPrecio3.API;
using System.Text.RegularExpressions;
using MejorPrecio3.MVC.Models;
using System.Collections.Generic;

namespace MejorPrecio3.MVC.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        SearchBestPrice api = new SearchBestPrice();

        [HttpGet("Register")]
        public IActionResult Register()
        {
            var model = new UserAdd();
            return View(model);
        }




        [HttpPost]
        public IActionResult Post( UserAdd userAdd)
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

        //[Route("history")]
        [HttpGet("history/{userId}")]
        public IActionResult GetHistory(Guid userId)
        {
            try{
                var searchHistory = api.GetSearchHistory(userId);
                List<HistoryViewModel> model = new List<HistoryViewModel>();
                int i=1;
                foreach(var search in searchHistory)
                {
                    var historyModel = new HistoryViewModel()
                    {
                        position = i,
                        name = search
                    };
                    i++;
                    model.Add(historyModel);
                }
            return View(model);
            }

            catch (Exception e){
                return StatusCode(412,e.Message);
            }
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
