using System;
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
        public IActionResult Login(LoginViewModel user)
        {
            var model = new LoginViewModel(){ Mail = user.Mail};
            if(!api.Login(user.Password, user.Mail))
            {
                ModelState.AddModelError("mail","El usuario o la contraseña son incorrectos");
                return View(model);
            }
            if(!api.IsUserVerified(user.Mail, user.Password))
            {
                ModelState.AddModelError("mail", "La cuenta no ha sido verificada");
                return View(model);
            }

            else
            {
                return StatusCode(401, "Los datos no corresponden a ningun usuario");
            }
        }


        [HttpPost("LogOut")]
        public IActionResult LogOff()
        {
            return StatusCode(501);
        }
    }
}