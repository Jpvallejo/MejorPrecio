using System;
using Microsoft.AspNetCore.Mvc;
using MejorPrecio3.Models;
using MejorPrecio3.API;
using System.Text.RegularExpressions;
using MejorPrecio3.MVC.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using MejorPrecio3.API.Services;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net;
using Newtonsoft.Json.Linq;

namespace MejorPrecio3.MVC.Controllers
{
    [Route("Account")]
    [Authorize]
    public class AccountController : Controller
    {

        private bool CheckRecaptcha(string response){
            string secretKey = "6LfE60IUAAAAANhRueMarM_eOBqLA8uhC55H9Cou"; 
            var client = new WebClient(); 
            var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, response)); 
            var obj = JObject.Parse(result); 
            var status = (bool)obj.SelectToken("success"); 
            return status;
        }
        AuthMessageSenderOptions emailOptions = new AuthMessageSenderOptions()
        {
            SendGridUser = "mejor_precio_3",
	        SendGridKey = "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"
        };
        SearchBestPrice api = new SearchBestPrice();



        [HttpGet]
        public IActionResult Index()
        {
            var searchHistory = api.GetSearchHistory(Guid.Parse(User.FindFirstValue(ClaimTypes.Sid)));
            var model = new AccountIndexViewModel(){
                name = User.Identity.Name,
                email = User.FindFirstValue(ClaimTypes.Email),
                history = new List<History>(searchHistory),
                modifyPassword = new ModifyPasswordViewModel()
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register()
        {
            var model = new UserAdd();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserAdd userAdd)
        {
            var captchaResponse = HttpContext.Request.Form["g-recaptcha-response"];
            if(!CheckRecaptcha(captchaResponse))
            {
                ModelState.AddModelError("captcha","El Captcha no ha sido ingresado correctamente");
                return View(userAdd);
            }
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
                    var token = api.GetUserToken(user.Mail);
                    var link = "http://" + this.Request.Host + this.Request.Path + "/Verify/?token=" + token;
                    await new EmailSender(emailOptions).SendEmailAsync(user.Mail, "Verificacion de cuenta", $"Confirme su cuenta haciendo click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Password", e.Message);
                    return View("Register", userAdd);
                }
                return View("ToVerify", userAdd);
            }
            else
            {
                return RedirectToAction("Index","Login",new LoginViewModel(){Mail=userAdd.Mail});
            }
        }

        [AllowAnonymous]
        [Route("Verify")]
        [HttpGet("{token}")]
        public IActionResult VerifyEmail(string token)
        {
            api.VerifyUser(Guid.Parse(token));
            return View("VerifiedUser");
        }


        [AllowAnonymous]
        [HttpGet("RecoveryPassword/{token}")]
        public IActionResult RestorePassword(string token)
        {
            var mail = api.GetEmailByToken(Guid.Parse(token));
            if (mail == String.Empty)
            {
                RedirectToAction("Register");
            }
            var model = new ModifyPasswordViewModel()
            {
                mail = mail
            };
            return View(model);
        }

        [AllowAnonymous]
        [Route("RestorePassword")]
        [HttpPost]
        public IActionResult RestorePassword(ModifyPasswordViewModel model)
        {
            if(!CheckRecaptcha(HttpContext.Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError("mail", "El Captcha no fue ingresado correctamente");
            }
            if (model.password != model.confirmPassword)
            {
                ModelState.AddModelError("password", "Las contraseñas no coinciden");
            }
            if (!ModelState.IsValid)
            {
                return View(new ModifyPasswordViewModel() { mail = model.mail });
            }
            try
            {
                api.ModifyPassword(model.mail, model.password);
                api.ModifyToken(model.mail);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("password", e.Message);
                return View(new ModifyPasswordViewModel() { mail = model.mail });
            }
            return View("ModifiedCorrectly");

        }

        [HttpPost("ModifyPassword")]
        public IActionResult ModifyPassword(ModifyPasswordViewModel model)
        {
            if(!CheckRecaptcha(HttpContext.Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError("mail", "El Captcha no fue ingresado correctamente");
            }
            if (model.password != model.confirmPassword)
            {
                ModelState.AddModelError("password", "Las contraseñas no coinciden");
            }
            if (!ModelState.IsValid)
            {
                return View(new ModifyPasswordViewModel() { mail = model.mail });
            }
            try
            {
                api.ModifyPassword(model.mail, model.password);
            }
            catch (Exception e)
        {
                ModelState.AddModelError("password", e.Message);
                return View(new ModifyPasswordViewModel() { mail = model.mail });
            }
            return View("ModifiedCorrectly");
        }

        [AllowAnonymous]
        [HttpPost("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPasswordAsync(string email)
        {
            var token = api.GetUserToken(email);
            var link = "http://" + this.Request.Host + this.Request.Path + "/" + token.ToString();
            await new EmailSender(emailOptions).SendEmailAsync(email, "Recupero de contraseña", $"Para reestablecer su contraseña haga click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
            return RedirectToAction("Index", "");
        }
    }
}