using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using MejorPrecio3.API;
using MejorPrecio3.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;

namespace MejorPrecio3.RESTApi.Models
{
    public class CustomAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string Name = "Custom";
        public CustomAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var header = this.Context.Request.Headers["X-CustomAuth"];

            if (header == StringValues.Empty)
            {
                return AuthenticateResult.NoResult();
            }

            var token = header.First();

            ClaimsPrincipal principal = ObtenerPrincipalPorToken(token);

            if (principal == null)
            {
                return AuthenticateResult.Fail("Token invalido");
            }

            var ticket = new AuthenticationTicket(principal, Name);
            return AuthenticateResult.Success(ticket);
        }

        private ClaimsPrincipal ObtenerPrincipalPorToken(string token)
        {
            var api = new SearchBestPrice();
            if (api.CheckValidToken(Guid.Parse(token)))
            {
                User user = api.GetUserByToken(Guid.Parse(token));
                var usernameClaim = new Claim(ClaimTypes.Name, user.Name);
                var roleClaim = new Claim(ClaimTypes.Role, user.Role);
                var emailClaim = new Claim(ClaimTypes.Email, user.Mail);
                var IdClaim = new Claim(ClaimTypes.Sid, user.Id.ToString());
                var identity = new ClaimsIdentity(new[] { usernameClaim, roleClaim, emailClaim, IdClaim }, "token");
                var principal = new ClaimsPrincipal(identity);

                return principal;
            }

            return null;
        }
    }

}
