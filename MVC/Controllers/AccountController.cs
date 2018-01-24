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

namespace MejorPrecio3.MVC.Controllers
{
    [Route("Account")]
    [Authorize]
    public class AccountController : Controller
    {
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
        public async Task<IActionResult> PostAsync(UserAdd userAdd)
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
                    var token = api.GetUserToken(user.Mail);
                    var link = "http://" + this.Request.Host + this.Request.Path + "/Verify/" + token;
                    await new EmailSender(emailOptions).SendEmailAsync(user.Mail, "Verificacion de cuenta", $"Confirme su cuenta haciendo click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("Password", e.Message);
                    return View("Register", userAdd);
                }
                return StatusCode(204);
            }
            else
            {
                return Content("El usuario ya existe");
            }
        }

        [Route("Verify")]
        [HttpGet("{token}")]
        public IActionResult VerifyEmail(string token)
        {
            api.VerifyUser(Guid.Parse(token));
            return View("VerifiedUser");
        }



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

        [Route("RestorePassword")]
        [HttpPost]
        public IActionResult RestorePassword(ModifyPasswordViewModel model)
        {
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
        
        
        [HttpGet("RecoveryPassword")]
        public IActionResult RecoveryPassword()
        {
            return View();
        }

        
        [HttpPost("RecoveryPassword")]
        public async Task<IActionResult> RecoveryPasswordAsync(string email)
        {
            var token = api.GetUserToken(email);
            var link = "http://" + this.Request.Host + this.Request.Path + "/" + token.ToString();
            await new EmailSender(emailOptions).SendEmailAsync(email, "Recupero de contraseña", $"Para reestablecer su contraseña haga click <a href='{HtmlEncoder.Default.Encode(link)}'>Aquí</a>");
            return RedirectToAction("Index", "");
        }

/*
        [HttpGet("history/{userId}")]
        public IActionResult GetHistory(Guid userId)
        {
            try{
                var searchHistory = api.GetSearchHistory(userId);
                List<HistoryViewModel> model = new List<HistoryViewModel>();
                int i = 1;
                foreach (var search in searchHistory)
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

            catch (Exception e)
            {
                return StatusCode(412, e.Message);
            }
        }

        [HttpPatch("ActualizarHistorial")]
        public IActionResult UpdateHistory([FromBody]User user)
        {
                    try
                    {
                        api.UpdateSearchHistory(user,new);
            }

                    catch (Exception e)
                    {
                return StatusCode(400, e.Message);
            }
            return StatusCode(200);
        }
                */
    }
}