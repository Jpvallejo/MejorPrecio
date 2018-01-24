using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MejorPrecio3.API;
using MejorPrecio3.RESTApi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MejorPrecio3.RESTApi.Controllers
{
    [Route("Login")]
    
    public class LoginController : Controller
    {
        SearchBestPrice api = new SearchBestPrice();

        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginViewModel model)
        {
            if (!api.Login(model.Password, model.Mail))
            {
                return StatusCode(400, "Los datos no corresponden a ningun usuario");
            }
            if (!api.IsUserVerified(model.Mail, model.Password))
            {
                return StatusCode(400, "La cuenta no ha sido verificada");
            }
            var user = api.GetUserByEmail(model.Mail);
            var emailClaim = new Claim(ClaimTypes.Email, model.Mail.ToString());
            var roleClaim = new Claim(ClaimTypes.Role, user.Role);
            var idClaim = new Claim(ClaimTypes.Sid, user.Id.ToString());
            var nameClaim = new Claim(ClaimTypes.Name, user.Name);
            var identity = new ClaimsIdentity(new[] { emailClaim, roleClaim, nameClaim, idClaim }, "cookie");
            var principal = new ClaimsPrincipal(identity);

            await this.HttpContext.SignInAsync(principal);

            User.FindFirstValue(ClaimTypes.Email);
            return StatusCode(200, "Ha iniciado sesion exitosamente");
        }


        [HttpPost("LogOut")]
        public async Task <IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync();

            return StatusCode(200, "Su sesion ha terminado");
        }
    }
}