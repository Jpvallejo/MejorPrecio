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
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody]UserLogin model)
        {
            if (!api.Login(model.Password, model.Mail))
            {
                return StatusCode(400, "Los datos no corresponden a ningun usuario");
            }
            if (!api.IsUserVerified(model.Mail, model.Password))
            {
                return StatusCode(400,"La cuenta no ha sido verificada");
            }
            var token = api.GetUserToken(model.Mail);
            return StatusCode(200, "Anote su Token, lo va a necesitar: " + token);
        }

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
                    var link = "http://" + this.Request.Host + this.Request.Path + "Verify/?token=" + token;
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
                return StatusCode(400, "El usuario ya existe");
            }
        }

        [AllowAnonymous]
        [Route("Verify")]
        [HttpGet("{token}")]
        public IActionResult VerifyEmail(string token)
        {
            api.VerifyUser(Guid.Parse(token));
            return StatusCode(200, "VerifiedUser");
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
            return StatusCode(200, "Se ha enviado un eMail a su correo electronico con instrucciones para recuperar su contraseña");
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
            return StatusCode(200, "Password modified correctly");
        }
    }
}