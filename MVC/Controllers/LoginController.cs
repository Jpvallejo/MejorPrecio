using System;
using System.Security.Claims;
using MejorPrecio3.API;
using MejorPrecio3.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MejorPrecio3.MVC.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        SearchBestPrice api = new SearchBestPrice();

        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }


        [HttpPost]
        public async System.Threading.Tasks.Task<IActionResult> LoginAsync(LoginViewModel user)
        {
            var model = new LoginViewModel() { Mail = user.Mail };
            if (!api.Login(user.Password, user.Mail))
            {
                ModelState.AddModelError("mail", "El usuario o la contraseña son incorrectos");
                return View(model);
            }
            if (!api.IsUserVerified(user.Mail, user.Password))
            {
                ModelState.AddModelError("mail", "La cuenta no ha sido verificada");
                return View(model);
            }

            var usernameClaim = new Claim(ClaimTypes.Email, model.Mail);
            var roleClaim = new Claim(ClaimTypes.Role, api.GetRoleForUser(model.Mail));
            var identity = new ClaimsIdentity(new[] { usernameClaim, roleClaim }, "cookie");
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            return RedirectToAction("Index","");

        }


        [HttpPost("LogOut")]
        public IActionResult LogOff()
        {
            return StatusCode(501);
        }
    }
}