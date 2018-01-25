using System;
using Microsoft.AspNetCore.Mvc;
using MejorPrecio3.Models;
using MejorPrecio3.API;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using MejorPrecio3.API.Services;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Threading.Tasks;
using System.Security.Claims;
using MejorPrecio3.RESTApi.Models;
using MejorPecio3.RESTApi.Models;

namespace MejorPrecio3.RESTApi.Controllers
{
    [Authorize]
    [Route("Account")]
    public class AccountController : Controller
    {
        SearchBestPrice api = new SearchBestPrice();

        AuthMessageSenderOptions emailOptions = new AuthMessageSenderOptions(){
            SendGridUser = "mejor_precio_3",
	        SendGridKey = "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"
        };

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody]UserAdd userAdd)
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
            if (!api.Exist(user.Mail))
            {
                try
                {
                    api.CreateUser(user);
                    var token = api.GetUserToken(user.Mail);
                    var link = "http://" + this.Request.Host + this.Request.Path + "/Verify/" + token;
                    await new EmailSender(emailOptions).SendEmailAsync(user.Mail, "Verificacion de cuenta", $"Confirme su cuenta haciendo click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
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
        }
*/
        
        [HttpPost("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPasswordAsync(string email)
        {
            var token = api.GetUserToken(email);
            var link = "http://" + this.Request.Host + this.Request.Path + "/" + token.ToString();
            await new EmailSender(emailOptions).SendEmailAsync(email,"Recupero de contraseña",$"Para reestablecer su contraseña haga click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
            return RedirectToAction("Index","");
        }

        [HttpPut("RestorePassword")]
        public IActionResult RestorePassword(ModifyPasswordViewModel model)
        {
            if(model.password != model.confirmPassword)
            {
                return StatusCode(400,"Las contraseñas no coinciden");
            }

            if(!ModelState.IsValid)
            {
                return StatusCode(400);
            }

            try
            {
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