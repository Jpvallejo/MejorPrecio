using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            var newModel = new LoginViewModel() { Mail = model.Mail };
            if (!api.Login(model.Password, model.Mail))
            {
                ModelState.AddModelError("mail", "El usuario o la contraseña son incorrectos");
                return View("Login",newModel);
            }
            if (!api.IsUserVerified(model.Mail, model.Password))
            {
                ModelState.AddModelError("mail", "La cuenta no ha sido verificada");
                return View("Login",newModel);
            }
            var user = api.GetUserByEmail(model.Mail);
            var emailClaim = new Claim(ClaimTypes.Email, model.Mail);
            var roleClaim = new Claim(ClaimTypes.Role, user.Role);
            var idClaim = new Claim(ClaimTypes.Sid, user.Id.ToString());
            var nameClaim = new Claim(ClaimTypes.Name, user.Name);
            var identity = new ClaimsIdentity(new[] { emailClaim, roleClaim, nameClaim, idClaim }, "cookie");
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            User.FindFirstValue(ClaimTypes.Email);
            return RedirectToAction("Index", "");

        }


        [HttpPost("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();

            return RedirectToAction("Index", "");
        }

    }
}