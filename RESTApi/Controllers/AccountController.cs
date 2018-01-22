using System;
using Microsoft.AspNetCore.Mvc;
using MejorPrecio3.Models;
using MejorPrecio3.API;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MejorPrecio3.API.Services;
using MejorPrecio3.RESTApi.Models;
using System.Text.Encodings.Web;

namespace MejorPrecio3.RESTApi.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {

        SearchBestPrice api = new SearchBestPrice();
        AuthMessageSenderOptions emailOptions = new AuthMessageSenderOptions(){
            SendGridUser = "mejor_precio_3",
	        SendGridKey = "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"
        };

        [HttpPost]
        public IActionResult Post([FromBody] UserAdd userAdd)
        {

            User user = new User()
            {
                Age = userAdd.Age,
                Verified = false,
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

        [Route("GetHistory/{userId}")]
        [HttpGet()]
        public IActionResult GetHistory(Guid userId)
        {
            try{
                var searchHistory = api.GetSearchHistory(userId);
                return Json(searchHistory);
            }

            catch (Exception e){
                return StatusCode(412,e.Message);
            }
        }
/*
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
        }*/





        
        [HttpPost("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPasswordAsync(string email)
        {
            var token = api.GetUserToken(email);
            var link = "http://" + this.Request.Host + this.Request.Path + "/" + token.ToString();
            await new EmailSender(emailOptions).SendEmailAsync(email,"Recupero de contraseña",$"Para reestablecer su contraseña haga click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
            return RedirectToAction("Index","");
        }


        [Route("RestorePassword")]
        [HttpPut]
        public IActionResult RestorePassword(ModifyPasswordViewModel model)
        {
            if(model.password != model.confirmPassword)
            {
                ModelState.AddModelError("password","Las contraseñas no coinciden");
            }
            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            try{
                api.ModifyPassword(model.mail, model.password);
            }
            catch (Exception e)
            {
                return StatusCode(400, e.Message);   
            }
            return Content("Password modified correctly");
        }



    }
}
